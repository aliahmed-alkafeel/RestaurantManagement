document.addEventListener("DOMContentLoaded", () => {

    const notifications = document.querySelectorAll(
        "#success-notification, #warning-notification"
    );

    notifications.forEach(notification => {

        setTimeout(() => {

            notification.classList.add("hide");

            setTimeout(() => {
                notification.remove();
            }, 300);

        }, 3000);

    });

});