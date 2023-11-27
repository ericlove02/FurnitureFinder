<?php
function OpenCon()
 {
    $dbhost = "localhost";
    $dbuser = "id21522484_alex9947";
    $dbpass = "quanG4$$";
    $db = "id21522484_chair";
    $conn = new mysqli($dbhost, $dbuser, $dbpass,$db) or die("Connect failed: %s\n". $conn -> error);

    return $conn;
 }

function CloseCon($conn)
 {
    $conn -> close();
 }
        $conn = OpenCon();
        
        // Perform SQL query to get all data from the table
        $sql = "SELECT * FROM furniture";
        $result = $conn->query($sql);
        
        $outp = $result->fetch_all(MYSQLI_ASSOC);
  
        echo json_encode($outp);
        
        CloseCon($conn);
?>