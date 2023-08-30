// Created By Sharad
// Positions are  : topCenter,topRight,topLeft,bottomCenter,bottomLeft,bottomRight
// Timeout : 1 sec - 1e3 ,2 sec 2e3, 3 sec 3e3 , 4 sec 4e3

function success(message, position) {
    iziToast.success({
        title: message+' '+'-',
        message: 'Success!',
        position: position,
        timeout: '2e3'
    });
}

function info(message, position) {
    iziToast.info({
        title: message + ' ' + '-',
        message: 'Information!',
        position: position,
        timeout: '2e3'
    });

}
function warning(message, position) {
    iziToast.warning({
        title: message+' '+'-',
        message: 'Warning!',
        position: position,
        timeout: '2e3'
    });
}

function error(message, position) {
    iziToast.error({
        title: message + ' ' + '-',
        message: 'Error!',
        position: position,
        timeout: '2e3'
    });
}