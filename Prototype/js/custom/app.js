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
  $rootScope.EmFields = ["Bane 1","Bane 2","Bane 5","Bane 6"];
  $rootScope.OmFields = ["Bane 3A","Bane 3B","Bane 4A","Bane 4B"];
  $rootScope.FmFields = ["Bane 7A","Bane 7B","Bane 7C","Bane 7D"];
})

.controller('HomeController', ['$scope', function ($scope) {
}])

.controller('CreateTournyController', ['$scope', function ($scope) {
}])

.controller('CreateFieldController', ['$scope', '$rootScope', function ($scope, $rootScope){

  $scope.newFieldName = "";

  $scope.newEm = false;

  $scope.newOm = false;

  $scope.newFm = false;

  $scope.createNewEmField = function(){
    $scope.newEm = !$scope.newEm;
  }

  $scope.submitEmField = function(newName) {
    $rootScope.EmFields.push(newName);
    $scope.newFieldName = "";
    $scope.createNewEmField();
  }

  $scope.removeEmField = function(index) {
    $rootScope.EmFields.splice(index, 1);
  }

  $scope.createNewOmField = function(){
    $scope.newOm = !$scope.newOm;
  }

  $scope.submitOmField = function(newName) {
    $rootScope.OmFields.push(newName);
    $scope.newFieldName = "";
    $scope.createNewOmField();
  }

  $scope.removeOmField = function(index) {
    $rootScope.OmFields.splice(index, 1);
  }

  $scope.createNewFmField = function(){
    $scope.newFm = !$scope.newFm;
  }

  $scope.submitFmField = function(newName) {
    $rootScope.FmFields.push(newName);
    $scope.newFieldName = "";
    $scope.createNewFmField();
  }

  $scope.removeFmField = function(index) {
    $rootScope.FmFields.splice(index, 1);
  }

 /* $scope.addField = function(index) {
    $rootScope.fields.push(index + 1);
  } */

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