.controller('CreateFieldsController', ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {

  /* Post & Get requests */ 
 /* $rootScope.Field.Id = 1;

  $http.get("http://localhost:50229/Field?id=" +  $rootScope.Field.Id)
  .success(function(fieldData)
  {
    $scope.Field = fieldData;
  }).error(function(err) 
  {
    $scope.error = err;
  })

  $http.post("http://localhost:50229/Field", params { hejhej: "hejgej", hej2: "htdyfg" })
  .success(function(fieldData)
  {
    $scope.Field = fieldData;
  }).error(function(err) 
  {
    $scope.error = err;
  }) */

  $scope.newFieldName = "";

  $scope.newEm = false;

  $scope.newOm = false;

  $scope.newFm = false; 

  /* 11man */
  $scope.createNewEmField = function() {
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

  /* 8man */
 $scope.createNewOmField = function() {
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

  /* 5man */
  $scope.createNewFmField = function() {
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

}])