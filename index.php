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

    if(isset($_GET["vibe_name"])) {
        $conn = OpenCon();
        
        $stmt = $conn->prepare("SELECT VIBE_ID FROM vibe WHERE VIBE_NAME = ?");
        $stmt->bind_param("s", $_GET['vibe_name']);
        $stmt->execute();
        $result = $stmt->get_result();
        $row = mysqli_fetch_array($result);
        
        $stmt2 = $conn->prepare("SELECT FUR_ID FROM furniturevibes WHERE VIBE_ID = ?");
        $stmt2->bind_param("s", $row[0]);
        $stmt2->execute();
        $result2 = $stmt2->get_result();
        $row2 = mysqli_fetch_array($result2);
        
        $stmt3 = $conn->prepare("SELECT * FROM furniture WHERE FUR_ID = ?");
        $stmt3->bind_param("s", $row2[0]);
        $stmt3->execute();
        $result3 = $stmt3->get_result();
        $outp = $result3->fetch_all(MYSQLI_ASSOC);
        
        $myJSON = json_encode($outp);
        echo $myJSON;
        CloseCon($conn);
    }
?>