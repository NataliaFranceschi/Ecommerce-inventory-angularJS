var app = angular.module('myApp', ['ngRoute']);

    app.config(function ($routeProvider){
        $routeProvider
            .when('/', {
                controller: 'LoginController',
                templateUrl: 'app/views/login.html'
            })
            .when('/inventory', {
                controller: 'ProductController',
                templateUrl: 'app/views/inventory.html'
            })
            .otherwise({ redirectTo: '/' });
    });

    app.controller('MainController', function($scope, $location) {
        $scope.isLogin = function() {
            return $location.path() === '/';
        };
    
        $scope.isInventory = function() {
            return $location.path() === '/inventory';
        };
    });