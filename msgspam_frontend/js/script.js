let selectedPlan = '';

function purchase(plan) {
    const modal = document.getElementById('modal');
    const title = document.getElementById('modal-title');
    const description = document.getElementById('modal-description');
    
    selectedPlan = plan; // Запоминаем выбранный план
    title.textContent = `Подписка: ${plan}`;
    description.innerHTML = getDescription(plan);
    modal.style.display = "block"; // Показываем модальное окно
}

function getDescription(plan) {
    const plans = {
      Trial: {
        title: 'Пробная подписка',
        description: `
          Ваш шанс оценить возможности нашего сервиса всего за <strong>19₽</strong>! 🎉<br><br>
          Воспользуйтесь всеми функциями, доступными в версии MSGspam Plus, и начинайте взаимодействовать с блогерами. 🤝
        `,
      },
      Plus: {
        title: 'Подписка MSGspam Plus',
        description: `
          Отличный выбор для пользователей, стремящихся повысить эффективность взаимодействия с блогерами. 💡✨
        `,
      },
      Ultimate: {
        title: 'Премиум подписка MSGspam Ultimate',
        description: `
          Решение для пользователей, настроенных серьезно и желающих максимально эффективно использовать возможности сервиса. 🚀💪
        `,
      },
    };

    return plans[plan] ? `<strong>${plans[plan].title}</strong> – ${plans[plan].description}` : '';
}
  
function closeModal() {
    document.getElementById('modal').style.display = "none"; // Скрываем модальное окно
}

// Закрыть модальное окно при клике вне окна
window.onclick = function(event) {
    const modal = document.getElementById('modal');
    if (event.target == modal) {
        closeModal();
    }
}

// Функция для выполнения поиска
function searchVideos() {
    const tag = document.getElementById('searchTag').value;
    alert(`Ищем видео с тегом: ${tag}`);
}

// Добавляем функцию для перенаправления
function goToPlanPage() {
    let planUrl = '';

    switch (selectedPlan) {
        case 'Trial':
            planUrl = 'Trial.html';
            break;
        case 'Plus':
            planUrl = 'Plus.html';
            break;
        case 'Ultimate':
            planUrl = 'Ultimate.html';
            break;
        default:
            return; // если план не выбран, ничего не делаем
    }

    window.location.href = planUrl; // Переходим на соответствующую страницу
}
