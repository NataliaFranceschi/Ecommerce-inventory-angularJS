app.controller('ProductController', function($scope, $http, $location) {
    $scope.products = [];
    $scope.token = localStorage.getItem('token')

    $scope.getProducts = function() {
        $http.get('http://localhost:5150/product', {
            headers: {
                'Authorization': 'Bearer ' + $scope.token
            }
        })
        .then(function(response) {
        $scope.products = response.data;
        })
        .catch(function(error) {
        console.error('Error fetching products:', error);
        });
    };      

    $scope.productData = {};

    $scope.createProduct = function() {
        $http.post('http://localhost:5150/product', $scope.productData, {
            headers: {
                'Authorization': 'Bearer ' + $scope.token
            }
        })
        .then(function(response) {
            $scope.getProducts();
            $scope.productData = {};
        })
        .catch(function(error) {
            console.error('Error creating product:', error);
        });
    };      
    
    $scope.remove = function(id, index) {
        $http.delete(`http://localhost:5150/product/${id}`, {
            headers: {
                'Authorization': 'Bearer ' + $scope.token
            }
        })
        .then(function(response) {
            $scope.products.splice(index,1);
        })
        .catch(function(error) {
            console.error('Error creating product:', error);
        });
    };        

    $scope.edit = function(product) {
        $scope.productData = product;
        
    };     

    $scope.updateProduct = function() {
        $http.put('http://localhost:5150/product', $scope.productData, {
            headers: {
                'Authorization': 'Bearer ' + $scope.token
            }
        })
        .then(function(response) {
            $scope.productData = {};
        })
        .catch(function(error) {
            console.error('Error creating product:', error);
        });
    };  

    $scope.logout = function() {
        localStorage.clear();
        $location.path('/')
    }

    $scope.getProducts();
});