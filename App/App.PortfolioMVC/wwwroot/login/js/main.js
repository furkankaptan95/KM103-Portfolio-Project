	document.addEventListener('DOMContentLoaded', function () {
        var alerts = document.querySelectorAll('.custom-alert-success, .custom-alert-error');
	alerts.forEach(function (alert) {
            if (alert) {
		setTimeout(function () {
			alert.style.transition = 'opacity 0.5s ease';
			alert.style.opacity = '0';
			setTimeout(function () {
				alert.remove();
			}, 500);
		}, 5000);
            }
        });
    });

(function ($) {

	"use strict";

	var fullHeight = function() {

		$('.js-fullheight').css('height', $(window).height());
		$(window).resize(function(){
			$('.js-fullheight').css('height', $(window).height());
		});

	};
	fullHeight();

	$(".toggle-password").click(function() {

	  $(this).toggleClass("fa-eye fa-eye-slash");
	  var input = $($(this).attr("toggle"));
	  if (input.attr("type") == "password") {
	    input.attr("type", "text");
	  } else {
	    input.attr("type", "password");
	  }
	});

})(jQuery);
