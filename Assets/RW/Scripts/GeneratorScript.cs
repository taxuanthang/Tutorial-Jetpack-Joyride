using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    private float scrrenWidthInPoints;

    public void Start()
    {
        float height = 2f * Camera.main.orthographicSize;
        scrrenWidthInPoints = height * Camera.main.aspect;

        StartCoroutine(GeneratorCheck());
    }

    void AddRoom(float farthestRoomEndX)
    {
        // pick a random room
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        // instantiate room
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]); //chỗ này tự nhiên lại ép kiểu trong khi Instantiate đã trả về kiểu của phần tử rồi mà
        // tìm GO floor trong room, lấy scale.x vdu: 1920p pixel
        float roomWidth = room.transform.Find("floor").localScale.x;    // hình như chỗ này ko nên dùng Find vì tốn tài nguyên, nên lưu trữ tham chiếu tới floor trong script của phòng
        // tính vị trí center của room mới = phải ngoài cùng + một nửa chiều rộng phòng mới
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
        // đặt vị trí phòng mới
        room.transform.position = new Vector3(roomCenter, 0, 0);
        currentRooms.Add(room);
    }

    void GenerateRoomIfRequired()
    {
        // Generate a list to store rooms to be removed
        List<GameObject> roomsToRemove = new List<GameObject>();

        bool addRooms = true;
        float playerX = transform.position.x;
        // point to indicat the leftmost let the coe know when to remove room
        float removeRoomX = playerX - scrrenWidthInPoints;
        // point to indicate the rightmost let the code know when to add room
        float addRoomX = playerX + scrrenWidthInPoints;

        float farthestRoomEndX = 0;
        foreach (var room in currentRooms)
        {
            float roomWidth = room.transform.Find("floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            // check if room start is after addRoomX
            if (roomStartX > addRoomX)
            {
                addRooms = false;
            }
            // check if room end is infront removeRoomX
            if (roomEndX < removeRoomX)
            {
                roomsToRemove.Add(room);
            }

            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        if (addRooms)
        {
            AddRoom(farthestRoomEndX);
        }
        
    }

    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GenerateRoomIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }
}
