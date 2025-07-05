using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class MapGenerator : Node
{
    public int floors { get; private set; } = 12;
    
    public const int PLACEMENT_RANDOMNESS = 5;
    public const int MAP_WIDTH = 7;
    public const int PATHS = 5;
    public const float MONSTER_ROOM_WEIGHT = 2.25F;
    public const float EVENT_ROOM_WEIGHT = 1.0F;
    public const float SHOP_ROOM_WEIGHT = 0.75F;
    public const float RESTSTOP_ROOM_WEIGHT = 0.5F;
    public const float TREASURE_ROOM_WEIGHT = 0.5F;
    
    public  List<List<RoomData>> mapData = new List<List<RoomData>>();
    
    Dictionary<RoomData.Type, float> randomRoomTypeWeights = new Dictionary<RoomData.Type, float>
    {
        {RoomData.Type.COMBAT, 0.0f},
        {RoomData.Type.EVENT, 0.0f},
        {RoomData.Type.REST, 0.0f},
        {RoomData.Type.TREASURE, 0.0f},
        {RoomData.Type.SHOP, 0.0f},
    };
    
    int randomRoomTypeTotalWeight = 0;
    
    public List<List<RoomData>> GenerateNewMap()
    {
        this.mapData = new List<List<RoomData>>(GenerateInitialGrid());
        List<int> startingPoints = new List<int>(GetRandomStartingPoints());

        foreach(int j in startingPoints)
        {
            int currentJ = j;

            for(int i = 0; i < this.floors - 1; i++)
            {
                currentJ = SetupConnection(i, currentJ);
            }
        }

        SetupBossRoom();
        SetupRandomRoomWeights();
        SetupRoomTypes();
    
        return mapData;
    }
    
    List<List<RoomData>> GenerateInitialGrid()
    {
        List<List<RoomData>> result = new List<List<RoomData>>();

        for(int i = 0; i < this.floors; i++)
        {
            List<RoomData> adjacentRooms = new List<RoomData>();

            for(int j = 0; j < MAP_WIDTH; j++)
            {
                RoomData currentRoom = new RoomData();
                currentRoom.row = i;
                currentRoom.column = j;
                currentRoom.nextRoomNumber = new List<int>();

                adjacentRooms.Add(currentRoom);
            }

            result.Add(adjacentRooms);
        }

        return result;
    }
    
    List<int> GetRandomStartingPoints()
    {
        List<int> results = new List<int>();
        int uniquePoints = 0;

        while(uniquePoints < 2)
        {
            uniquePoints = 0;
            results = new List<int>();

            for(int i = 0; i < PATHS; i++)
            {
                int startingPoint = RunManager.instance.currentRun.rng.RandiRange(0, MAP_WIDTH-1);

                if(!results.Contains(startingPoint))
                {
                    uniquePoints += 1;
                }

                results.Add(startingPoint);
            } 
        }

        return results;
    }
    
    int SetupConnection(int i, int j)
    {
        RoomData nextRoom = null;
        RoomData currentRoom = this.mapData[i][j] as RoomData;

        while(nextRoom == null || WouldCrossPaths(i, j, nextRoom))
        {
            int randomJ = Math.Clamp(RunManager.instance.currentRun.rng.RandiRange(j-1,j+1), 0, MAP_WIDTH - 1);
            nextRoom = this.mapData[i+1][randomJ];
        }

        currentRoom.nextRoomNumber.Add(nextRoom.column);

        return nextRoom.column;
    }
    
    bool WouldCrossPaths(int i, int j, RoomData nextRoom)
    {
        RoomData leftRoom = null;
        RoomData rightRoom = null;

        if(j > 0)
        {
            leftRoom = this.mapData[i][j-1] as RoomData;
        }

        if(j < MAP_WIDTH-1)
        {
            rightRoom = this.mapData[i][j+1] as RoomData;
        }

        if(rightRoom != null && nextRoom.column > j)
        {
            foreach(int roomNumber in rightRoom.nextRoomNumber)
            {
                if(roomNumber < nextRoom.column) 
                {
                    return true; 
                }
            }
        }

        if(leftRoom != null && nextRoom.column < j)
        {
            foreach(int roomNumber in leftRoom.nextRoomNumber)
            {
                if(roomNumber > nextRoom.column) 
                {
                    return true; 
                }
            }
        }

        return false;
    }
    
    void SetupBossRoom()
    {
        int middle = Mathf.FloorToInt(MAP_WIDTH /2);
        RoomData bossRoom = this.mapData[this.floors-1][middle] as RoomData;
        bossRoom.type = RoomData.Type.BOSS;
        bossRoom.battleData = new BattleData();
        bossRoom.battleData.SetData(RunManager.instance.GetRandomBattleforTier(2));

        for(int j = 0; j < MAP_WIDTH; j++)
        {
            RoomData currentRoom = mapData[this.floors-2][j] as RoomData;
            
            if(currentRoom.nextRoomNumber.Any())
            {
                currentRoom.nextRoomNumber.Clear();
                currentRoom.nextRoomNumber.Add(bossRoom.column);
            }
        }
    }
    
    void SetupRandomRoomWeights()
    {
        randomRoomTypeWeights[RoomData.Type.COMBAT] = MONSTER_ROOM_WEIGHT;
        randomRoomTypeWeights[RoomData.Type.EVENT] = MONSTER_ROOM_WEIGHT + EVENT_ROOM_WEIGHT;
        randomRoomTypeWeights[RoomData.Type.REST] = MONSTER_ROOM_WEIGHT + EVENT_ROOM_WEIGHT + RESTSTOP_ROOM_WEIGHT;
        randomRoomTypeWeights[RoomData.Type.TREASURE] = MONSTER_ROOM_WEIGHT + EVENT_ROOM_WEIGHT + RESTSTOP_ROOM_WEIGHT + TREASURE_ROOM_WEIGHT;
        randomRoomTypeWeights[RoomData.Type.SHOP] = MONSTER_ROOM_WEIGHT + EVENT_ROOM_WEIGHT + RESTSTOP_ROOM_WEIGHT + TREASURE_ROOM_WEIGHT + SHOP_ROOM_WEIGHT;

        randomRoomTypeTotalWeight = (int) randomRoomTypeWeights[RoomData.Type.SHOP];
    }
    
    void SetupRoomTypes()
    {
        foreach(RoomData room in mapData[0])
        {
            if(room.nextRoomNumber.Count > 0)
            {
                room.type = RoomData.Type.COMBAT;
                room.battleData = new BattleData();
                room.battleData.SetData(RunManager.instance.GetRandomBattleforTier(0));
            }
        }

        foreach(RoomData room in mapData[this.floors/2])
        {
            if(room.nextRoomNumber.Count > 0)
            {
                room.type = RoomData.Type.TREASURE;
            }
        }

        foreach(RoomData room in mapData[this.floors-2])
        {
            if(room.nextRoomNumber.Count > 0)
            {
                room.type = RoomData.Type.REST;
            }
        }

        foreach(List<RoomData> floor in mapData)
        {
            foreach(RoomData room in floor)
            {
                foreach(int roomNumber in room.nextRoomNumber)
                {
                    if(this.mapData[room.row+1][roomNumber].type == RoomData.Type.NONE)
                    {
                        SetRoomRandomly(this.mapData[room.row+1][roomNumber]);
                    }
                }
            }
        }
    }
    
    void SetRoomRandomly(RoomData room)
    {
        bool campfireBelow4 = true;
        bool consecutiveCampfire = true;
        bool consecutiveShop = true;
        bool campfireon13 = true;

        RoomData.Type typeCandidate = RoomData.Type.NONE;

        while (campfireBelow4 || consecutiveCampfire || consecutiveShop || campfireon13)
        {
            typeCandidate = GetRandomRoombyTypeWeight();

            bool isCampfire = (typeCandidate == RoomData.Type.REST);
            bool hasCampfireParent = RoomHasParentofType(room, RoomData.Type.REST);
            bool isShop = (typeCandidate == RoomData.Type.SHOP);
            bool hasShopParent = RoomHasParentofType(room, RoomData.Type.SHOP);

            campfireBelow4 = isCampfire && room.row < 3;
            consecutiveCampfire = isCampfire && hasCampfireParent;
            consecutiveShop = isShop && hasShopParent;
            campfireon13 = isCampfire && room.row == 12;
        }

        room.type = typeCandidate;

        if (typeCandidate == RoomData.Type.COMBAT)
        {
            int tier = 0;

            if (room.row > 2)
            {
                tier++;
            }

            room.battleData = new BattleData();
            room.battleData.SetData(RunManager.instance.GetRandomBattleforTier(tier));
        }

        if (typeCandidate == RoomData.Type.EVENT)
        {
            int tier = 0;
            if (room.row > this.floors/2)
            {
                tier++;
            }
            
            room.eventData = RunManager.instance.GetRandomEventforTier(tier);
        }
    }
    
    RoomData.Type GetRandomRoombyTypeWeight()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        float roll = rng.RandfRange(0.0f, randomRoomTypeTotalWeight);

        foreach(KeyValuePair<RoomData.Type, float> roomTypes in randomRoomTypeWeights)
        {
            if(roomTypes.Value > roll)
            {
                return roomTypes.Key;
            }
        }

        return RoomData.Type.COMBAT;
    }
    
    bool RoomHasParentofType(RoomData room, RoomData.Type type)
    {
        List<RoomData> parents = new List<RoomData>();

        if(room.column > 0 && room.row > 0)
        {
            RoomData parentCandidate = mapData[room.row-1][room.column-1] as RoomData;

            if(parentCandidate.nextRoomNumber.Contains(room.column))
            {
                parents.Add(parentCandidate);
            }
        }

        if(room.row > 0)
        {
            RoomData parentCandidate = mapData[room.row-1][room.column] as RoomData;

            if(parentCandidate.nextRoomNumber.Contains(room.column))
            {
                parents.Add(parentCandidate);
            }
        }

        if(room.column < MAP_WIDTH-1 && room.row > 0)
        {
            RoomData parentCandidate = mapData[room.row-1][room.column+1] as RoomData;

            if(parentCandidate.nextRoomNumber.Contains(room.column))
            {
                parents.Add(parentCandidate);
            }
        }

        foreach(RoomData parent in parents)
        {
            if(parent.type == type)
                return true;
        }

        return false;
    }
}
