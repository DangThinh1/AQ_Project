$(function(){
    $('.aq-nav-profile > .nav-list > .nav-item > a[href^="/Profile/' + location.pathname.split("/")[2] + '"]').parent().addClass('active');
}); 