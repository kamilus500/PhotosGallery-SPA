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