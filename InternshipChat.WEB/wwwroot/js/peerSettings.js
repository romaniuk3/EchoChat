let userId = null;
let localStream = null;
let videoGrid;
let myPeer;


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
        const remoteVideo = document.getElementById("remote-video");

        call.on("stream", remoteStream => {
            remoteVideo.srcObject = remoteStream;
        })
    })
}

async function startLocalStream() {
    const localVideo = document.getElementById("local-video");

    await navigator.mediaDevices.getUserMedia({
        video: true,
        audio: false
    }).then(stream => {
        localVideo.srcObject = stream;
        localStream = stream;
    })
}


function connectNewUser(userId) {
    const remoteVideo = document.getElementById("remote-video");
    const call = myPeer.call(userId, localStream)

    call.on('stream', remoteStream => {
        remoteVideo.srcObject = remoteStream;
    });
}


function destroyConnection() {
    localStream.getTracks().forEach(function (track) {
        if (track.readyState === 'live') {
            track.stop();
        }
    });
    myPeer.destroy();
    myPeer = null;
    localStream = null;
}
