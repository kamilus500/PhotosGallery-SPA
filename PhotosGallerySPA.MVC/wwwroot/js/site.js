let closeEventClass = 'close-event';
let xlModalClass = 'modal-xl';
function loadPartial(pathUrl, data = null) {
    $('#loading-spinner').show();

    $('main').load(pathUrl, data, function () {
        $('#loading-spinner').hide();
    });
}

function loadPartialInModal(pathUrl) {
    $.get(pathUrl).done(function (data) {
        $('.modal-body').html(data);
    });
}

function loadImageModal(data) {

    //Change size
    //$('.modal-dialog').addClass(xlModalClass);

    //Add image
    let htmlresult = `<img src="data:image/jpeg;base64,${data}" class="full-image"/>`; 
    $('.modal-body').html(htmlresult);

    //event after click close button
    //$('.modal-header .btn-close').addClass(closeEventClass);
}

function cleanImageModalClasses() {
    if ($(closeEventClass)) {
        setTimeout(function () {
            $('.modal-header .btn-close').removeClass(closeEventClass);
            $('.modal-dialog').removeClass(xlModalClass);
        }, 700)
    }
}

function hideModal() {
    let modal = document.getElementById('globalModal');
    modal.style.display = 'none';

    let modalBackground = document.querySelector('.modal-backdrop.fade.show');
    modalBackground.remove();
}

function loadConfirmation(message, pathUrl, actionUrl = null, data = null) {
    
    if (confirm(message)) {
        if (actionUrl) {
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function () {
                    loadPartial(pathUrl);
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
        }
    }
}