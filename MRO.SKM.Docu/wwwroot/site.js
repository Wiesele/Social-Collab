function resizeIFrameToFitContent( iFrame ) {
    iFrame.style.height = iFrame.contentWindow.document.body.scrollHeight + 500 + "px";
}

function autosizeIframe(name){
    window.addEventListener('DOMContentLoaded', function(e) {
        let iFrame = document.getElementById(name);
        
        resizeIFrameToFitContent(iFrame);
    });


    setTimeout(function(){
        let tmp = document.getElementById(name);
        tmp.style.height = tmp.contentWindow.document.body.scrollHeight + 500 + "px";
    }, 100);
}