using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneratorScript : MonoBehaviour
{
    [Header("Generate Rooms")]
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    private float screenWidthInPoints;

    [Header("Generate Objects")]
    public GameObject[] availableObjects;
    public List<GameObject> objects;

    public float objectsMinDistance = 5.0f;
    public float objectsMaxDistance = 10.0f;

    public float objectsMinY = -1.4f;
    public float objectsMaxY = 1.4f;

    public float objectsMinRotation = -45.0f;
    public float objectsMaxRotation = 45.0f;


    public void Start()
    {
        float height = 2f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;

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
        float removeRoomX = playerX - screenWidthInPoints;
        // point to indicate the rightmost let the code know when to add room
        float addRoomX = playerX + screenWidthInPoints;

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
            GenerateObjectsIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }

    void AddObject(float lastObjectX)
    {
        // Pick a random Index
        int randomIndex = Random.Range(0, availableObjects.Length);
        // Instantiate
        GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);
        // calculat new object pos based on previous object plus random rang
        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
        float randomY = Random.Range(objectsMinY, objectsMaxY);
        obj.transform.position = new Vector3(objectPositionX, randomY, 0);
        // rotate objct by random range
        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
        // add to list
        objects.Add(obj);
    }

    void GenerateObjectsIfRequired()
    {
        // initial remove and add checkPoint for object
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;
        //
        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (var obj in objects)
        {
            //3
            float objX = obj.transform.position.x;
            //4
            farthestObjectX = Mathf.Max(farthestObjectX, objX);
            // check if out of range
            if (objX < removeObjectsX)
            {
                objectsToRemove.Add(obj);
            }
        }
        // Delete
        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }
        // check if the latest object is in the adding range
        if (farthestObjectX < addObjectX)
        {
            AddObject(farthestObjectX);
        }
    }



}
