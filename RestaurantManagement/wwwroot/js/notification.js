document.addEventListener("DOMContentLoaded", () => {

    const notification = document.getElementById("success-notification");

    if (!notification)
        return;

    setTimeout(() => {

        notification.classList.add("hide");

        setTimeout(() => {
            notification.remove();
        }, 300);

    }, 3000);
});