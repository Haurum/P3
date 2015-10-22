angular.module('tournyplanner', ['ngRoute'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.
    when('/', {
      templateUrl: 'templates/home.html',
      controller: 'HomeController'
    }).
    when('/addTourny', {
      templateUrl: 'templates/OpretNT.html',
      controller: 'CreateTournyController'
    }).
    when('/divisions', {
      templateUrl: 'templates/tournament-divs.html',
      controller: 'DivisonController'
    }).
    when('/divisions/detail', {
      templateUrl: 'templates/tournament-divs-detail.html',
      controller: 'DivisonDetailController'
    }).
    otherwise({
      redirectTo: '/'
    });
}])

.run(function($rootScope) {
  $rootScope.divisions = ["Noobs", "Gorrilaer", "Gulleroderne", "Master", "Challenger"];
})

.controller('HomeController', ['$scope', function ($scope) {
}])

.controller('CreateTournyController', ['$scope', function ($scope) {
  console.log("bhisrhbs");
}])

.controller('DivisonController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
  
  $scope.newDivName = "";
  
  $scope.new = false;
  
  $scope.createNew = function() {
    $scope.new = !$scope.new;
  }
  $scope.submit = function(newName) {
    $rootScope.divisions.push(newName);
    $scope.newDivName = "";
    $scope.createNew();
  }
  
  $scope.remove = function(index) {
    $rootScope.divisions.splice(index, 1);
  }
  
  $scope.gotoDivison = function(currDiv) {
    $rootScope.currDivision = currDiv;
    $location.url("/divisions/detail");
  }
}])

.controller('DivisonDetailController', ['$scope', '$rootScope', function ($scope, $rootScope) {
  $scope.division = $rootScope.currDivision;
}])

