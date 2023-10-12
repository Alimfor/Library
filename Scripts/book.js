$(document).ready(
    function () {
        $("#bt_test").click(
            $.ajax
                ({
                    type: "GET",
                    url: "http://localhost:5180/api/library/all",
                    success: function (data) {
                        console.log(data);
                    },
                    error: function (data) {
                        console.log("error");
                    }
                })
        );
    }
);