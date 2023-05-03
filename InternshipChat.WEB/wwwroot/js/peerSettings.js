const roomId = "room1";
let userId = null;
let localStream = null;
let videoGrid;
let myPeer;
const Peers = {};


function createPeer() {
    myPeer = new Peer();
    subscribeOnPeerEvents();

    return new Promise((res, rej) => {
        myPeer.on("open", id => {
            res(id);
        })
    })
}

function subscribeOnPeerEvents() {
    myPeer.on("call", call => {
        call.answer(localStream)

        const userVideo = document.createElement('video');

        call.on("stream", userVideoStream => {
            addVideoStream(userVideo, userVideoStream);
        })
    })
}

async function startLocalStream() {
    videoGrid = document.getElementById("my-video-grid");
    const myVideo = document.createElement('video')
    myVideo.muted = true;

    navigator.mediaDevices.getUserMedia({
        video: true
    }).then(stream => {
        addVideoStream(myVideo, stream);

        localStream = stream;
    })
}

const addVideoStream = (video, stream) => {
    video.srcObject = stream;
    video.addEventListener("loadedmetadata", () => {
        video.play();
    })
    videoGrid.appendChild(video)
}


function connectNewUser(userId) {
    const userVideo = document.createElement('video');
    const call = myPeer.call(userId, localStream)

    call.on('stream', userVideoStream => {
        addVideoStream(userVideo, userVideoStream)
    });

    call.on('close', () => {
        userVideo.remove();
    })

    Peers[userId] = call;
}

function disconnectUser(userId) {
    if (Peers[userId]) Peers[userId].close();
}
