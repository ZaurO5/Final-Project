$(document).ready(function () {
    $(document).on('click', '.color', function (e) {
        e.preventDefault();
        $('.color').removeClass('selected');
        $(this).addClass('selected');
        $('#selectedColor').val($(this).data('color'));
        console.log('Selected color:', $('#selectedColor').val());
    });

    $(document).on('click', '.size', function (e) {
        e.preventDefault();
        $('.size').removeClass('selected');
        $(this).addClass('selected');
        $('#selectedSize').val($(this).data('size'));
        console.log('Selected size:', $('#selectedSize').val());
    });

    $('#quantity').on('input', function () {
        const max = window.productData ? window.productData.stockCount : $(this).attr('max');
        const value = parseInt($(this).val()) || 1;
        const validValue = Math.min(Math.max(value, 1), max);
        $(this).val(validValue);
        $('.quantity-error').toggle(validValue > max);
    });

    function showMessage(selector, message, type = 'success') {
        const $messageDiv = $(selector);
        if ($messageDiv.length) {
            $messageDiv
                .text(message)
                .css({
                    'display': 'block',
                    'background-color': type === 'success' ? '#d4edda' : '#f8d7da',
                    'color': type === 'success' ? '#155724' : '#721c24',
                    'border': type === 'success' ? '1px solid #c3e6cb' : '1px solid #f5c6cb'
                })
                .delay(3000)
                .fadeOut(500);
        } else {
            console.warn('Message element not found:', selector);
        }
    }

    $('.add-to-basket').click(function (e) {
        e.preventDefault();

        const productId = window.productData ? window.productData.productId : $(this).closest('form').find('[name="productId"]').val();
        const color = $('#selectedColor').val();
        const size = $('#selectedSize').val();
        const quantity = $('#quantity').val();

        let hasError = false;

        if (!color) {
            showMessage('.product-messages', 'Пожалуйста, выберите цвет', 'error');
            hasError = true;
        }
        if (!size) {
            showMessage('.product-messages', 'Пожалуйста, выберите размер', 'error');
            hasError = true;
        }
        if (quantity > window.productData?.stockCount) {
            showMessage('.product-messages', 'Количество превышает доступный запас', 'error');
            hasError = true;
        }

        if (!hasError) {
            $.post('/Basket/AddProduct', {
                productId: productId,
                colorId: parseInt(color),
                sizeId: parseInt(size),
                quantity: parseInt(quantity)
            })
                .done(response => {
                    if (response.statusCode === 200) {
                        showMessage('.product-messages', 'Товар успешно добавлен в корзину');
                        setTimeout(() => {
                            window.location.href = '/Basket';
                        }, 1000);
                    } else {
                        showMessage('.product-messages', response.description, 'error');
                    }
                })
                .fail(error => {
                    const errorMsg = error.responseJSON?.description || 'Произошла ошибка при добавлении в корзину';
                    showMessage('.product-messages', errorMsg, 'error');
                });
        }
    });

    $(document).on('click', '.basket-remove-btn', function () {
        const $item = $(this).closest('.basket-wrapper');
        const basketId = $(this).data('basket-id');
        const productId = $(this).data('product-id');
        const colorId = $(this).data('color-id');
        const sizeId = $(this).data('size-id');

        $.ajax({
            url: '/Basket/Delete',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                basketId: basketId,
                productId: productId,
                colorId: colorId,
                sizeId: sizeId
            }),
            success: function (response) {
                if (response.statusCode === 200) {
                    $item.remove();
                    showMessage('.basket-messages', 'Товар удален из корзины');
                    updateTotalPrice();
                } else {
                    showMessage('.basket-messages', response.description, 'error');
                }
            },
            error: function () {
                showMessage('.basket-messages', 'Ошибка при удалении товара', 'error');
            }
        });
    });

    $('.update-all-btn').on('click', function () {
        const updatedProducts = [];
        let isValid = true;

        $('.quantity-input').each(function () {
            const $input = $(this);
            const basketId = $input.data('basket-id');
            const productId = $input.data('product-id');
            const colorId = $input.data('color-id');
            const sizeId = $input.data('size-id');
            const newQuantity = parseInt($input.val());
            const maxQuantity = parseInt($input.attr('max'));
            const originalValue = parseInt($input.data('original-value'));
            const productTitle = $input.closest('.basket-wrapper').find('.basket-item-title').text();

            if (isNaN(newQuantity) || newQuantity < 1) {
                showMessage('.basket-messages', `Количество для "${productTitle}" должно быть не менее 1`, 'error');
                $input.val(originalValue);
                isValid = false;
                return false;
            }

            if (newQuantity > maxQuantity) {
                showMessage('.basket-messages', `Количество для "${productTitle}" превышает доступный запас (${maxQuantity})`, 'error');
                $input.val(originalValue);
                isValid = false;
                return false;
            }

            if (newQuantity !== originalValue) {
                updatedProducts.push({
                    BasketId: basketId,
                    ProductId: productId,
                    ColorId: colorId,
                    SizeId: sizeId,
                    Count: newQuantity
                });
            }
        });

        if (!isValid) return;

        if (updatedProducts.length > 0) {
            $.ajax({
                url: '/Basket/UpdateCart',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(updatedProducts),
                success: function (response) {
                    if (response.statusCode === 200) {
                        showMessage('.basket-messages', 'Корзина успешно обновлена');
                        $('.quantity-input').each(function () {
                            const $input = $(this);
                            const newValue = parseInt($input.val());
                            $input.data('original-value', newValue);

                            const $wrapper = $input.closest('.basket-wrapper');
                            const pricePerUnit = parseFloat($wrapper.find('.basket-item-price').data('price'));
                            $wrapper.find('.basket-item-price').text('$' + (pricePerUnit * newValue).toFixed(2));
                        });
                        updateTotalPrice();
                    } else {
                        showMessage('.basket-messages', response.description, 'error');
                        $('.quantity-input').each(function () {
                            const $input = $(this);
                            $input.val($input.data('original-value'));
                        });
                    }
                },
                error: function (xhr) {
                    const errorMsg = xhr.responseJSON?.description || 'Ошибка при обновлении корзины';
                    showMessage('.basket-messages', errorMsg, 'error');
                    $('.quantity-input').each(function () {
                        const $input = $(this);
                        $input.val($input.data('original-value'));
                    });
                }
            });
        } else {
            showMessage('.basket-messages', 'Нет изменений для обновления');
        }
    });

    $('.quantity-input').on('input', function () {
        const $input = $(this);
        const max = parseInt($input.attr('max'));
        const min = parseInt($input.attr('min'));
        let value = parseInt($input.val()) || min;

        if (value < min) value = min;
        if (value > max) value = max;
        $input.val(value);
    });

    function updateTotalPrice() {
        let total = 0;
        $('.basket-wrapper').each(function () {
            const priceText = $(this).find('.basket-item-price').text();
            const price = parseFloat(priceText.replace(/[^0-9.]/g, ''));
            total += price;
        });
        $('.total-price span').text('$' + total.toFixed(2));
    }

    $(document).on('click', '.checkout-btn', function (e) {
        e.preventDefault();
        const $btn = $(this);
        $btn.prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i> Processing...');

        if ($('.basket-wrapper').length === 0) {
            showMessage('.basket-messages', 'Your basket is empty', 'error');
            $btn.prop('disabled', false).text('Checkout');
            return;
        }

        $.post('/Payment/Pay')
            .done(function (response) {
                if (response && response.url) {
                    window.location.href = response.url;
                } else {
                    showMessage('.basket-messages', 'Failed to get payment URL', 'error');
                }
            })
            .fail(function (xhr) {
                const errorMsg = xhr.responseJSON?.description ||
                    xhr.statusText ||
                    'Payment processing failed';
                showMessage('.basket-messages', errorMsg, 'error');
            })
            .always(() => {
                $btn.prop('disabled', false).text('Checkout');
            });
    });

});