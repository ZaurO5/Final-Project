$(function () {
    // Инициализация выбора цвета
    $(document).on('click', '.color', function (e) {
        e.preventDefault();
        $('.color').removeClass('selected');
        $(this).addClass('selected');
        $('#selectedColor').val($(this).data('color'));
        console.log('Selected color:', $('#selectedColor').val());
    });

    // Инициализация выбора размера
    $(document).on('click', '.size', function (e) {
        e.preventDefault();
        $('.size').removeClass('selected');
        $(this).addClass('selected');
        $('#selectedSize').val($(this).data('size'));
        console.log('Selected size:', $('#selectedSize').val());
    });

    // Обработка количества
    $('#quantity').on('input', function () {
        const max = window.productData.stockCount;
        const value = parseInt($(this).val()) || 1;
        const validValue = Math.min(Math.max(value, 1), max);

        $(this).val(validValue);
        $('.quantity-error').toggle(validValue > max);
    });

    // Добавление в корзину
    $('.add-to-basket').click(function () {
        const productId = window.productData.productId;
        const color = $('#selectedColor').value;
        const size = $('#selectedSize').value;
        const quantity = $('#quantity').value;

        let hasError = false;

        // Валидация
        if (!color) {
            $('.color-error').show();
            hasError = true;
        }
        if (!size) {
            $('.size-error').show();
            hasError = true;
        }
        if (quantity > window.productData.stockCount) {
            $('.quantity-error').text('Quantity exceeds stock').show();
            hasError = true;
        }

        if (!hasError) {
            $.post('/Basket/AddProduct', {
                productId: productId,
                color: color,
                size: size,
                quantity: quantity
            })
                .done(response => {
                    if (response.statusCode === 200) {
                        window.location.reload();
                    } else {
                        alert(response.description);
                    }
                })
                .fail(error => {
                    console.error('Error:', error);
                    alert('Произошла ошибка: ' + error.responseJSON?.description);
                });
        }
    });
});
