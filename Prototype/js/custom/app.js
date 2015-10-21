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
      when('/addFields', {
      templateUrl: 'templates/baner.html',
      controller: 'CreateFieldController'
    }).
    otherwise({
      redirectTo: '/'
    });
}])

.run(function($rootScope) {
  $rootScope.divisions = ["Noobs", "Gorrilaer", "Gulleroderne", "Master", "Challenger"];
})

.run(function($rootScope) {
  $rootScope.fields = ["Bane 7A","Bane 7B","Bane 7C","Bane 7D"];
})

.controller('HomeController', ['$scope', function ($scope) {
}])

.controller('CreateTournyController', ['$scope', function ($scope) {
}])

.controller('CreateFieldController', ['$scope', function ($scope){
}])

.controller('DivisonController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
  
  $scope.newDivName = "";
  
  $scope.new = false;

  $scope.newFieldName = "";
  
  $scope.createNew = function() {
    $scope.new = !$scope.new;
  }
  $scope.submit = function(newName) {
    $rootScope.divisions.push(newName);
    $scope.newDivName = "";
    $scope.createNew();
  }

  $scope.submitField = function(newName) {
    $rootScope.fields.push(newName);
    $scope.newFieldName = "";
    $scope.createNew();
  }
  
  $scope.remove = function(index) {
    $rootScope.divisions.splice(index, 1);
  }

  $scope.removeField = function(index) {
    $rootScope.fields.splice(index, 1);
  }

  $scope.addField = function(index) {
    $rootScope.fields.spilce(index + 1);
  }
  
  $scope.gotoDivison = function(currDiv) {
    $rootScope.currDivision = currDiv;
    $location.url("/divisions/detail");
  }
}])

.controller('DivisonDetailController', ['$scope', '$rootScope', function ($scope, $rootScope) {
  $scope.division = $rootScope.currDivision;
}])