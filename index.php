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
        $vibe_row = $result->fetch_assoc();

        if ($vibe_row) {
            // Get the associated FUR_ID values from the furniturevibes table
            $stmt2 = $conn->prepare("SELECT FUR_ID FROM furniturevibes WHERE VIBE_ID = ?");
            $stmt2->bind_param("i", $vibe_row['VIBE_ID']);
            $stmt2->execute();
            $result2 = $stmt2->get_result();
        
            // Use a prepared statement with IN clause to retrieve all associated furniture
            $placeholders = implode(',', array_fill(0, $result2->num_rows, '?'));
            $stmt3 = $conn->prepare("SELECT * FROM furniture WHERE FUR_ID IN ($placeholders)");
            
            // Bind parameters dynamically
            $params = array();
            $types = "";
            while ($row2 = $result2->fetch_assoc()) {
                $params[] = &$row2['FUR_ID'];
                $types .= "i";
            }
            array_unshift($params, $types);
            call_user_func_array(array($stmt3, 'bind_param'), $params);
        
            $stmt3->execute();
            $result3 = $stmt3->get_result();
            $outp = $result3->fetch_all(MYSQLI_ASSOC);
        
            // Output your data as needed
            echo json_encode($outp);
        } else {
            // Handle the case where the given vibe_name doesn't exist
            echo "Vibe not found";
        }
        
        // Close your statements and connection as needed
        $stmt->close();
        $stmt2->close();
        $stmt3->close();
        
        CloseCon($conn);
    }
?>