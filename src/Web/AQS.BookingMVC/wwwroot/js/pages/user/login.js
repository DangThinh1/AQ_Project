var loginObj = (function (window, doc, $) {
    "use strict";
    var url = "/User/Login";
    let auth2 = null;
    $(doc).ready(function () {
    });
    $(document).on("submit", "#loginForm", function () {
        $.post(url, $('#loginForm').serialize(), function (res) {
            if (res.isSuccessStatusCode) {
                window.location.reload();
            }
            else {
                $('#spLoginError').html(res.message);
            }
        })
        return false;
    });

    window.LoginFacebook = function () {
        FB.getLoginStatus(function (response) {
            console.log(response.status);
            if (response.status === "connected") {
                var accessToken = response.authResponse.accessToken;
                var userID = response.authResponse.userID;
                var model = {
                    UserId: userID,
                    AccessToken: accessToken
                };
                var url = "/User/FacebookLogin";
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: model,
                    beforeSend: function () {
                    },
                    success: function (result) {
                        if (result) {
                            window.location.reload();
                        }
                    },
                    error: function (xhr) { // if error occured
                        console.log("verify fail");
                    },
                    complete: function () {
                    },
                    dataType: 'html'
                });
            }
            else {
                console.log("disconnected");
            }
        });
    }

    window.fbAsyncInit = function () {
        FB.init({
            appId: '659249811560723',
            cookie: true,
            xfbml: true,
            version: 'v6.0'
        });
        FB.AppEvents.logPageView();
    };

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement(s); js.id = id;
        js.src = "https://connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    function loadGGUsername(model) {
        $.post(HomeURLObj.loginGGURL, { model: model }, (result) => {
            if (result) {
                window.location.reload();
            }
        });
    };
    var googleUser = {};
    var startGoogleLoginApp = function () {
        gapi.load('auth2', function () {
            // Retrieve the singleton for the GoogleAuth library and set up the client.
            auth2 = gapi.auth2.init({
                client_id: '869263867429-dceju2m0k70lv6mra3h3alst1v8vj378.apps.googleusercontent.com',
                cookiepolicy: 'single_host_origin',
                // Request scopes in addition to 'profile' and 'email'
                //scope: 'additional_scope'
            });
            attachSignin(document.getElementById('customBtn'));
        });
    };

    function attachSignin(element) {
        ///console.log(element.id);
        auth2.attachClickHandler(element, {},
            function (googleUser) {
                //document.getElementById('name').innerText = "Signed in: " +
                //    googleUser.getBasicProfile().getName();
                var profile = googleUser.getBasicProfile();
                var model = {
                    GoogleId: profile.getId(),
                    Email: profile.getEmail(),
                    FullName: profile.getName(),
                    Givenname: profile.getGivenName(),
                    FamilyName: profile.getFamilyName(),
                    ImageUrl: profile.getImageUrl()
                };
                loadGGUsername(model);
            }, function (error) {
                alert(JSON.stringify(error, undefined, 2));
            });
    };

    return {        
        startGoogleLoginApp,
    }

})(window, document, jQuery);
