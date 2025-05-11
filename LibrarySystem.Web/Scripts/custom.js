$(document).ready(function () {
    // Example: Show a confirmation dialog before signing out
    $(".btn-danger").on("click", function (e) {
        if (!confirm("Are you sure you want to sign out?")) {
            e.preventDefault();
        }
    });
});
