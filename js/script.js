function purchase(plan) {
    const modal = document.getElementById('modal');
    const title = document.getElementById('modal-title');
    const description = document.getElementById('modal-description');
    
    title.textContent = `Подписка: ${plan}`;
    description.textContent = getDescription(plan);
    modal.style.display = "block"; // Показываем модальное окно
}

function getDescription(plan) {
    switch (plan) {
        case 'Trial':
            return 'Это пробная подписка, которая позволит вам разово протестировать работу сервиса за 19₽.';
        case 'Plus':
            return 'Стандартная подписка, которая стоит 299₽ в месяц. Она включает все основные функции.';
        case 'Ultimate':
            return 'Премиальная подписка за 699₽ в месяц с расширенными возможностями и поддержкой.';
        default:
            return '';
    }
}

function closeModal() {
    document.getElementById('modal').style.display = "none"; // Скрываем модальное окно
}

// Закрывать модальное окно при клике вне окна
window.onclick = function(event) {
    const modal = document.getElementById('modal');
    if (event.target == modal) {
        closeModal();
    }
}
