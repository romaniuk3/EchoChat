// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [/\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/, /\.mp3$/, /\.svg$/ ];
const offlineAssetsExclude = [ /^service-worker\.js$/ ];

async function onInstall(event) {
    console.info('Service worker: Install');

    // Fetch and cache all matching items from the assets manifest
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));
    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));
}

async function onActivate(event) {
    console.info('Service worker: Activate');

    // Delete unused caches
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}
async function onFetch(event) {
    let cachedResponse = null;
    if (event.request.method === 'GET') {
        if (event.request.url.includes('/chathub/')) {
            return;
        }
        // Check if the request URL matches the API endpoint
        if (event.request.url.includes('/api/')) {
            const cache = await caches.open(cacheName);
            // Check if the API response is already cached
            cachedResponse = await cache.match(event.request);
            if (cachedResponse) {
                // If cached response exists, return it immediately
                return cachedResponse;
            } else {
                // If not cached, fetch the response from the network and cache it
                const response = await fetch(event.request.clone());
                if (response && response.status === 200) {
                    cache.put(event.request, response.clone());
                }
                return response;
            }
        } else {
            // For all other navigation and static file requests, serve from cache
            const shouldServeIndexHtml = event.request.mode === 'navigate';
            const request = shouldServeIndexHtml ? 'index.html' : event.request;
            const cache = await caches.open(cacheName);
            cachedResponse = await cache.match(request);
        }
    }

    return cachedResponse || fetch(event.request);
}