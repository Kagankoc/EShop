$(function () {

    if ($("a.confirmDeletion").length)
    {
        $("a.confirmDeletion").click(() =>
        {
            if (!confirm("Confirm Deletion")) return false;
        });
    }

    if ($("div.notification").length) {

        setTimeout(() => {
            $("div.notification").fadeOut();
        }, 2000);

    }

});

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function(e) {
            $("img#imgFile").attr("src", e.target.result).width(200).height(200);
        };

        reader.readAsDataURL(input.files[0]);

    }
}