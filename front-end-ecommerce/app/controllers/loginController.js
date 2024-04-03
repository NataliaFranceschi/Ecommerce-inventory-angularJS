app.controller('LoginController', function($scope, $http, $location) {
    $scope.login = {};
    
    $scope.getToken = function() {
        $http.post('http://localhost:5150/login', $scope.login)
        .then(function(response) {
        localStorage.setItem('token', response.data.token);
        $location.path('/inventory')
        })
        .catch(function(error) {
        console.error('Error fetching user:', error);
        $scope.errorMessage = error.data.message;
        });
    };      
});