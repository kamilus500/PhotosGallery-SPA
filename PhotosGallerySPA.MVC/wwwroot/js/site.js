function loadPartial(pathUrl, data = null) {
    $('main').load(pathUrl, data);
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