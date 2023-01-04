let socket = io("http://ictrobot.hknu.ac.kr:8089");

let localVideo;
let cameraButton;
let camerasSelect;
let muteButton;

let localPeerConnection;
let localStream;

let cameraOff = false;
let muted = false;

let roomName;

async function joinRoom(name) {
    initField();
    await initCall();
    roomName = name
    socket.emit("join_room", roomName);
}

function initField() {
    localVideo = document.getElementById("localVideo");
    cameraButton = document.getElementById("camera");
    camerasSelect = document.getElementById("cameras");
    muteButton = document.getElementById("mute");

    cameraButton.addEventListener("click", handleCameraClick);
    camerasSelect.addEventListener("input", handleCameraChange);
    muteButton.addEventListener("click", handleMuteClick);
}

async function initCall() {
    await getMedia();
    makeConnection();
}

async function getMedia(deviceId) {
    const initialConstrains = {
        audio: true,
        video: { facingMode: "user" },
    };
    const cameraConstraints = {
        audio: true,
        video: { deviceId: { exact: deviceId } },
    };
    try {
        localStream = await navigator.mediaDevices.getUserMedia(
            deviceId ? cameraConstraints : initialConstrains
        );
        localVideo.srcObject = localStream;
        if (!deviceId) {
            await getCameras();
        }
    } catch (e) {
        console.log(e);
    }
}

async function getCameras() {
    try {
        const devices = await navigator.mediaDevices.enumerateDevices();

        const cameras = devices.filter((device) => device.kind === "videoinput");
        const currentCamera = localStream.getVideoTracks()[0];
        cameras.forEach((camera) => {
            const option = document.createElement("option");
            option.value = camera.deviceId;
            option.innerText = camera.label;
            if (currentCamera.label === camera.label) {
                option.selected = true;
            }
            camerasSelect.appendChild(option);
        });
    } catch (e) {
        console.log(e);
    }
}

function handleMuteClick() {
    localStream
        .getAudioTracks()
        .forEach((track) => (track.enabled = !track.enabled));
    if (!muted) {
        muteButton.innerText = "Unmute";
        muted = true;
    } else {
        muteButton.innerText = "Mute";
        muted = false;
    }
}

function handleCameraClick() {
    localStream
        .getVideoTracks()
        .forEach((track) => (track.enabled = !track.enabled));
    if (cameraOff) {
        cameraButton.innerText = "Turn Camera Off";
        cameraOff = false;
    } else {
        cameraButton.innerText = "Turn Camera On";
        cameraOff = true;
    }
}

async function handleCameraChange() {
    await getMedia(camerasSelect.value);
    if (localPeerConnection) {
        const videoTrack = localStream.getVideoTracks()[0];
        const videoSender = localPeerConnection
            .getSenders()
            .find((sender) => sender.track.kind === "video");
        videoSender.replaceTrack(videoTrack);
    }
}

// Socket

socket.on("welcome", async () => {
    const offer = await localPeerConnection.createOffer();
    localPeerConnection.setLocalDescription(offer);
    console.log("Sent the offer.");
    socket.emit("offer", offer, roomName);
});

socket.on("offer", async (offer) => {
    console.log("Received the offer.");
    localPeerConnection.setRemoteDescription(offer);
    const answer = await localPeerConnection.createAnswer();
    localPeerConnection.setLocalDescription(answer);
    socket.emit("answer", answer, roomName);
    console.log("Sent the answer.");
});

socket.on("answer", (answer) => {
    console.log("Received the answer.");
    localPeerConnection.setRemoteDescription(answer);
});

socket.on("ice", (ice) => {
    console.log("Received candidate.");
    localPeerConnection.addIceCandidate(ice);
});

// RTC

function makeConnection() {
    localPeerConnection = new RTCPeerConnection({
        iceServers: [
            {
                urls: [
                    "stun:stun.l.google.com:19302",
                    "stun:stun1.l.google.com:19302",
                    "stun:stun2.l.google.com:19302",
                    "stun:stun3.l.google.com:19302",
                    "stun:stun4.l.google.com:19302",
                ],
            },
        ],
    });
    localPeerConnection.addEventListener("icecandidate", handleIce);
    localPeerConnection.addEventListener("addstream", handleAddStream);
    localStream
        .getTracks()
        .forEach((track) => localPeerConnection.addTrack(track, localStream));
}

function handleIce(data) {
    console.log("Sent candidate.");
    socket.emit("ice", data.candidate, roomName);
}

function handleAddStream(data) {
    console.log("Received stream.");
    const remoteVideo = document.getElementById("remoteVideo");
    remoteVideo.srcObject = data.stream;
}