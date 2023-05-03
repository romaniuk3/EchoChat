const roomId = "room1";
let userId = null;
let localStream = null;
let videoGrid;
const Peers = {};

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7021/chathub").build();

const myPeer = new Peer()

myPeer.on("open", id => {
    userId = id;
})

myPeer.on("call", call => {
    call.answer(localStream)

    const userVideo = document.createElement('video');

    call.on("stream", userVideoStream => {
        addVideoStream(userVideo, userVideoStream);
    })
})

async function StartSignaling() {
    const startSignalR = async () => {
        await connection.start();
        await connection.invoke("JoinRoom", roomId, userId)
    }
    startSignalR();
}

connection.on('user-connected', id => {
    if (userId === id) return;
    console.log(`User connected ${id}`);
    connectNewUser(id, localStream)
});

connection.on('user-disconnected', id => {
    console.log("disconnected user ", id);

    if (Peers[id]) Peers[id].close();
})

async function StartLocalStream() {
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

const connectNewUser = (userId, localStream) => {
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