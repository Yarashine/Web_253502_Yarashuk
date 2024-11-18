document.addEventListener('DOMContentLoaded', function () {
    // Проверяем, что URL страницы содержит "Catalog"
    if (window.location.pathname.startsWith('/Catalog/')) {
        function bindPaginationEvents() {
            // Отслеживаем клики по ссылкам пагинации
            var pagerLinks = document.querySelectorAll('.page-link');

            pagerLinks.forEach(function (link) {
                link.addEventListener('click', function (event) {
                    event.preventDefault(); // Отменяем стандартное поведение ссылки

                    // Получаем номер страницы и категорию из URL
                    var urlParams = new URLSearchParams(link.search);
                    var pageNo = urlParams.get('pageNo');
                    var category = link.pathname.split('/')[2];

                    // Выполняем асинхронный запрос
                    fetch(`/Catalog/${category}?pageNo=${pageNo}`, {
                        method: 'GET',
                        headers: {
                            'X-Requested-With': 'XMLHttpRequest' // Добавляем заголовок, чтобы запрос был распознан как Ajax
                        }
                    })
                        .then(response => response.text()) // Ожидаем HTML-контент
                        .then(html => {
                            // Обновляем контейнер с продуктами
                            document.getElementById('product-list-container').innerHTML = html;
                            // После обновления контента снова привязываем обработчики событий
                            bindPaginationEvents();
                        })
                        .catch(error => console.error('Ошибка загрузки:', error));
                });
            });
        }

        // Привязываем события при загрузке страницы
        bindPaginationEvents();
    }
});
