function localToUtc(value) {
    if (!value) return "";
    const date = new Date(value);
    return isNaN(date.getTime()) ? "" : date.toISOString();
}

function utcToLocal(value) {
    if (!value) return "";
    const date = new Date(value);
    if (isNaN(date.getTime())) return "";
    const p = n => String(n).padStart(2, "0");
    return `${date.getFullYear()}-${p(date.getMonth() + 1)}-${p(date.getDate())}T${p(date.getHours()) }:${p(date.getMinutes())}:${p(date.getSeconds())}`;
}

document.addEventListener("DOMContentLoaded", () => {

    document.querySelectorAll("input[data-utc-field][data-utc]").forEach(input => {
        if (input.dataset.utc) input.value = utcToLocal(input.dataset.utc);
    });

    document.querySelectorAll("form[data-utc-form]").forEach(form => {

        form.addEventListener("submit", function (event) {
            event.preventDefault();

            if (window.jQuery && !$(form).valid()) return;

            const formData = new FormData(form);

            form.querySelectorAll("input[data-utc-field]").forEach(input => {
                formData.set(input.name, localToUtc(input.value));
            });

            const tempForm = document.createElement("form");
            tempForm.method = "POST";
            tempForm.action = form.action;

            for (const [key, value] of formData.entries()) {
                const hidden = document.createElement("input");
                hidden.type = "hidden";
                hidden.name = key;
                hidden.value = value;
                tempForm.appendChild(hidden);
            }

            document.body.appendChild(tempForm);
            tempForm.submit();
        });
    });
});