using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using EFT;
using Comfort.Common;
using Diz;

namespace UnityVision
{
    class Main : UnityEngine.MonoBehaviour
    {
        public void Load()
        {
            GameObjectHolder = new GameObject();
            GameObjectHolder.AddComponent<Main>();
            DontDestroyOnLoad(GameObjectHolder);
        }

        public Main() { }
        #region variables
        private GameObject GameObjectHolder;
        private IEnumerable<Player> _playerInfo;
        private float _viewdistance = 1200f;
        private static Player _localPlayer;
        private Vector3 camPos;
        private float _playNextUpdateTime;
        public float _playersNextUpdateTime;
        public float _playerEspUpdateInterval = 2f;
        public float _espUpdateInterval = 100f;
        public float _ExtractionNextUpdateTime;
        public Color USEC;
        public Color Bear;
        public Color Scav;
        public float _PlayerCount;
        public bool _LightCreated;
        public IEnumerable<ExfiltrationPoint> _extractPoints;

        public bool _showESP = true;
        public bool _showSkel = true;

        public float rs = 1;
        public float gs = 1;
        public float bs = 1;
        public float rm = 0;
        public float gm = 0;
        public float bm = 1;
        public float cr = 0;
        public float cg = 1;
        public float cb = 1;
        public float ca = 1;
        public float dr = 1f;
        public float dg = 0.5f;
        public float db = 0.1f;
        public float da = 1f;
        public float mr = 1f;
        public float mg = 1f;
        public float mb = 1f;
        public float ma = 1f;
        public float r = 1;
        public float g = 0;
        public float b = 0;
        #endregion

        //Code in here runs once and only once. Use for initializing shit
        private void Start()
        {
            //clear variables?
            Init();
        }

        //Coe in here runs once per frame. Run aimbot code or w/e in here. This is strictly NON-VISUAL CODE
        private void Update()
        {
            //Show ESP keybind
            if (Input.GetKeyDown(KeyCode.Home))
            {
                _showESP = true;
            }
            else if(Input.GetKeyDown(KeyCode.End))
            {
                _showESP = false;
            }

            //Show skeleton keybind
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                _showSkel = true;
            }
            else if (Input.GetKeyDown(KeyCode.PageDown))
            {
                _showSkel = false;
            }
        }

        //If you want to draw on the screen for esp and shit, run that code in here. This is strictly VISUAL/DRAWING/MENU CODE
        private void OnGUI()
        {
            //draw logo in top left corner of screen
            DrawLabel();

            //show ESP
            if (_showESP)
            {
                if (Time.time >= this._playersNextUpdateTime)
                {
                    this._playersNextUpdateTime = Time.time + this._playerEspUpdateInterval;
                }

                if (Time.time >= _ExtractionNextUpdateTime)
                {
                    this._extractPoints = FindObjectsOfType<ExfiltrationPoint>();
                    this._ExtractionNextUpdateTime = Time.time + _playerEspUpdateInterval;
                }

                DrawPlayers();
                ExtractionESP();
            }
            
        }

        private void DrawLabel()
        {
            UnityEngine.GUI.Label(new UnityEngine.Rect(10, 10, 200, 40), "UnityVision 0x0001");
        }

        private void DrawPlayers()
        {
            foreach (Player player in GetGameWorld().RegisteredPlayers)
            {
                if (player == _localPlayer) continue;
                if (!player.IsVisible) continue;

                #region Bone Vectors
                var playerRightPalmVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.PlayerBones.RightPalm.position).x,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.RightPalm.position).y,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.RightPalm.position).z);
                var playerLeftPalmVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.PlayerBones.LeftPalm.position).x,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.LeftPalm.position).y,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.LeftPalm.position).z);
                var playerLeftShoulderVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.PlayerBones.LeftShoulder.position).x,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.LeftShoulder.position).y,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.LeftShoulder.position).z);
                var playerRightShoulderVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.PlayerBones.RightShoulder.position).x,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.RightShoulder.position).y,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.RightShoulder.position).z);
                var playerNeckVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.PlayerBones.Neck.position).x,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.Neck.position).y,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.Neck.position).z);
                var playerCenterVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.PlayerBones.Pelvis.position).x,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.Pelvis.position).y,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.Pelvis.position).z);
                var playerRightThighVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.PlayerBones.RightThigh2.position).x,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.RightThigh2.position).y,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.RightThigh2.position).z);
                var playerLeftThighVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.PlayerBones.LeftThigh2.position).x,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.LeftThigh2.position).y,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.LeftThigh2.position).z);
                var playerRightFootVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.PlayerBones.KickingFoot.position).x,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.KickingFoot.position).y,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.KickingFoot.position).z);
                var playerBoundingVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.Transform.position).x,
                    Camera.main.WorldToScreenPoint(player.Transform.position).y,
                    Camera.main.WorldToScreenPoint(player.Transform.position).z);
                var playerHeadVector = new Vector3(
                    Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).x,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).y,
                    Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).z);
                var playerLeftFootVector = new Vector3(
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 18)).x,
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 18)).y,
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 18)).z
                    );
                var playerLeftElbow = new Vector3(
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 91)).x,
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 91)).y,
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 91)).z
                    );
                var playerRightElbow = new Vector3(
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 112)).x,
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 112)).y,
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 112)).z
                    );
                var playerLeftKnee = new Vector3(
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 17)).x,
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 17)).y,
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 17)).z
                    );
                var playerRightKnee = new Vector3(
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 22)).x,
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 22)).y,
                    Camera.main.WorldToScreenPoint(GetBonePosByID(player, 22)).z
                    );
                #endregion

                Color playerColor = GetPlayerColor(player.Side, player);

                float distance = Vector3.Distance(Camera.main.transform.position, player.Transform.position);

                int sizeOfFont = 15;

                if (playerBoundingVector.z > 0.01)
                {
                    if (distance <= 50)
                    {
                        sizeOfFont = 15;
                    }
                    else if (distance < 100 && distance > 50)
                    { sizeOfFont = 13; }
                    else if (distance > 100 && distance <= 200)
                    { sizeOfFont = 11; }
                    else if (distance > 200 && distance <= 300)
                    { sizeOfFont = 9; }
                    else
                    { sizeOfFont = 9; }

                    var guistyle = new GUIStyle
                    { fontSize = sizeOfFont };

                    guistyle.normal.textColor = playerColor;

                    var isAi = player.Profile.Info.RegistrationDate <= 0;
                    int playerLevel = player.Profile.Info.Level;
                    var playerName = isAi ? "AI" : player.Profile.Info.Nickname;
                    float playerHealth = player.HealthController.HealthRate; //Not sure if this will work
                    float headPos = Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).y + 30f;
                    //string playerText = $"[{(int)playerHealth}%] {playerName} [{(int)distance}m] [{playerLevel}]"; OLD
                    string playerText = $"[{playerName}] {Environment.NewLine} [{playerLevel}] {Environment.NewLine} [{(int)playerHealth}%] {Environment.NewLine} [{(int)distance} m]"; //new

                    #region Draw Text
                    var Style = GUI.skin.GetStyle(playerText).CalcSize(new GUIContent(playerText));
                    GUI.Label(new Rect(playerBoundingVector.x - Style.x / 2f, (float)Screen.height - headPos - 1, 300f, 50f), playerText, guistyle);
                    #endregion

                    //@TODO - Toggle for drawing bones
                    #region Draw Bones
                    if(_showSkel)
                    {
                        GuiHelper.DrawLine(new Vector2(playerNeckVector.x, (float)Screen.height - playerNeckVector.y), new Vector2(playerCenterVector.x, (float)Screen.height - playerCenterVector.y), playerColor);
                        GuiHelper.DrawLine(new Vector2(playerLeftShoulderVector.x, (float)Screen.height - playerLeftShoulderVector.y), new Vector2(playerLeftElbow.x, (float)Screen.height - playerLeftElbow.y), playerColor);
                        GuiHelper.DrawLine(new Vector2(playerRightShoulderVector.x, (float)Screen.height - playerRightShoulderVector.y), new Vector2(playerRightElbow.x, (float)Screen.height - playerRightElbow.y), playerColor);
                        GuiHelper.DrawLine(new Vector2(playerLeftElbow.x, (float)Screen.height - playerLeftElbow.y), new Vector2(playerLeftPalmVector.x, (float)Screen.height - playerLeftPalmVector.y), playerColor);
                        GuiHelper.DrawLine(new Vector2(playerRightElbow.x, (float)Screen.height - playerRightElbow.y), new Vector2(playerRightPalmVector.x, (float)Screen.height - playerRightPalmVector.y), playerColor);
                        GuiHelper.DrawLine(new Vector2(playerRightShoulderVector.x, (float)Screen.height - playerRightShoulderVector.y), new Vector2(playerLeftShoulderVector.x, (float)Screen.height - playerLeftShoulderVector.y), playerColor);
                        GuiHelper.DrawLine(new Vector2(playerLeftKnee.x, (float)Screen.height - playerLeftKnee.y), new Vector2(playerCenterVector.x, (float)Screen.height - playerCenterVector.y), playerColor);
                        GuiHelper.DrawLine(new Vector2(playerRightKnee.x, (float)Screen.height - playerRightKnee.y), new Vector2(playerCenterVector.x, (float)Screen.height - playerCenterVector.y), playerColor);
                        GuiHelper.DrawLine(new Vector2(playerLeftKnee.x, (float)Screen.height - playerLeftKnee.y), new Vector2(playerLeftFootVector.x, (float)Screen.height - playerLeftFootVector.y), playerColor);
                        GuiHelper.DrawLine(new Vector2(playerRightKnee.x, (float)Screen.height - playerRightKnee.y), new Vector2(playerRightFootVector.x, (float)Screen.height - playerRightFootVector.y), playerColor);
                    }
                    
                    #endregion

                }
            }
        }

        private Color GetPlayerColor(EPlayerSide side, Player player)
        {
            Bear.r = r;
            Bear.g = g;
            Bear.b = b;
            Bear.a = 1;
            USEC.r = rm;
            USEC.g = gm;
            USEC.b = bm;
            USEC.a = 1;
            Scav.r = rs;
            Scav.g = gs;
            Scav.b = bs;
            Scav.a = 1;
            if (IsVisible(player.gameObject, getBonePos(player)))
            {
                return Color.magenta;
            }
            else
            {
                switch (side)
                {
                    case EPlayerSide.Bear:
                        return Bear;
                    case EPlayerSide.Usec:
                        return USEC;
                    case EPlayerSide.Savage:
                        return Scav;
                    default:
                        return Color.white;
                }
            }
        }

        private double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));
        }

        private void GetLocalPlayer()
        {
            foreach (Player player in FindObjectsOfType<Player>())
            {
                if (player == null) continue;

                if (EPointOfView.FirstPerson == player.PointOfView && player != null)
                {
                    _localPlayer = player;
                }
            }
        }

        private GameWorld GetGameWorld()
        {
            if (Singleton<GameWorld>.Instantiated)
            {
                _PlayerCount = Singleton<GameWorld>.Instance.RegisteredPlayers.Count;
                foreach (Player player in Singleton<GameWorld>.Instance.RegisteredPlayers)
                {
                    if (player.gameObject.GetComponent<PlayerOwner>() != null)
                    {
                        _localPlayer = player;
                    }
                }
                return Singleton<GameWorld>.Instance;
            }
            else
            {
                _PlayerCount = 0;
                _LightCreated = false;
                return null;
            }
        }

        private bool IsVisible(GameObject obj, Vector3 Position)
        {
            RaycastHit raycastHit;
            return Physics.Linecast(GetShootPos(), Position, out raycastHit) && raycastHit.collider && raycastHit.collider.gameObject.transform.root.gameObject == obj.transform.root.gameObject;
        }

        public Vector3 getBonePos(Player inP)
        {
            int bid = idtobid(ibid.Neck);
            return this.GetBonePosByID(inP, bid);
        }

        public static Vector3 GetShootPos()
        {
            if (_localPlayer == null)
            {
                return Vector3.zero;
            }
            Player.FirearmController firearmController = _localPlayer.HandsController as Player.FirearmController;
            if (firearmController == null)
            {
                return Vector3.zero;
            }
            return firearmController.Fireport.position;
        }

        public Vector3 GetBonePosByID(Player p, int id)
        {
            Vector3 result;
            try
            {
                result = SkeletonBonePos(p.PlayerBones.AnimatedTransform.Original.gameObject.GetComponent<PlayerBody>().SkeletonRootJoint, id);
            }
            catch (Exception)
            {
                result = Vector3.zero;
            }
            return result;
        }

        public static Vector3 SkeletonBonePos(Diz.Skinning.Skeleton sko, int id)
        {
            return sko.Bones.ElementAt(id).Value.position;
        }

        public enum ibid
        {
            Head,
            Neck,
            Chest,
            Stomach
        }

        public int idtobid(ibid bid)
        {
            switch (bid)
            {
                case ibid.Neck:
                    return 132;
                case ibid.Chest:
                    return 36;
                case ibid.Stomach:
                    return 29;
                default:
                    return 133;
            }
        }

        private string extractionNametoSimpleName(string extractionName)
        {
            // Factory
            if (extractionName.Contains("exit (3)"))
                return "Cellars";
            else if (extractionName.Contains("exit (1)"))
                return "Gate 3";
            else if (extractionName.Contains("exit (2)"))
                return "Gate 0";
            else if (extractionName.Contains("exit_scav_gate3"))
                return "Gate 3";
            else if (extractionName.Contains("exit_scav_camer"))
                return "Blinking Light";
            else if (extractionName.Contains("exit_scav_office"))
                return "Office";

            // Woods
            else if (extractionName.Contains("eastg"))
                return "East Gate";
            else if (extractionName.Contains("scavh"))
                return "House";
            else if (extractionName.Contains("deads"))
                return "Dead Mans Place";
            else if (extractionName.Contains("var1_1_constant"))
                return "Outskirts";
            else if (extractionName.Contains("scav_outskirts"))
                return "Outskirts";
            else if (extractionName.Contains("water"))
                return "Outskirts Water";
            else if (extractionName.Contains("boat"))
                return "The Boat";
            else if (extractionName.Contains("mountain"))
                return "Mountain Stash";
            else if (extractionName.Contains("oldstation"))
                return "Old Station";
            else if (extractionName.Contains("UNroad"))
                return "UN Road Block";
            else if (extractionName.Contains("var2_1_const"))
                return "UN Road Block";
            else if (extractionName.Contains("gatetofactory"))
                return "Gate to Factory";
            else if (extractionName.Contains("RUAF"))
                return "RUAF Gate";

            // Shoreline
            else if (extractionName.Contains("roadtoc"))
                return "Road to Customs";
            else if (extractionName.Contains("lighthouse"))
                return "Lighthouse";
            else if (extractionName.Contains("tunnel"))
                return "Tunnel";
            else if (extractionName.Contains("wreckedr"))
                return "Wrecked Road";
            else if (extractionName.Contains("deadend"))
                return "Dead End";
            else if (extractionName.Contains("housefence"))
                return "Ruined House Fence";
            else if (extractionName.Contains("gyment"))
                return "Gym Entrance";
            else if (extractionName.Contains("southfence"))
                return "South Fence Passage";
            else if (extractionName.Contains("adm_base"))
                return "Admin Basement";

            // Customs
            else if (extractionName.Contains("administrationg"))
                return "Administration Gate";
            else if (extractionName.Contains("factoryfar"))
                return "Factory Far Corner";
            else if (extractionName.Contains("oldazs"))
                return "Old Gate";
            else if (extractionName.Contains("milkp_sh"))
                return "Shack";
            else if (extractionName.Contains("beyondfuel"))
                return "Beyond Fuel Tank";
            else if (extractionName.Contains("railroadtom"))
                return "Railroad to Mil Base";
            else if (extractionName.Contains("_pay_car"))
                return "V-Exit";
            else if (extractionName.Contains("oldroadgate"))
                return "Old Road Gate";
            else if (extractionName.Contains("sniperroad"))
                return "Sniper Road Block";
            else if (extractionName.Contains("warehouse17"))
                return "Warehouse 17";
            else if (extractionName.Contains("factoryshacks"))
                return "Factory Shacks";
            else if (extractionName.Contains("railroadtotarkov"))
                return "Railroad to Tarkov";
            else if (extractionName.Contains("trailerpark"))
                return "Trailer Park";
            else if (extractionName.Contains("crossroads"))
                return "Crossroads";
            else if (extractionName.Contains("railroadtoport"))
                return "Railroad to Port";

            // Interchange
            else if (extractionName.Contains("NW_Exfil"))
                return "North West Extract";
            else if (extractionName.Contains("SE_Exfil"))
                return "South East Extract";
            else
                return extractionName;
        }

        public void ExtractionESP()
        {
            foreach (var point in _extractPoints)
            {
                if (point != null)
                {
                    float distanceToObject = Vector3.Distance(Camera.main.transform.position, point.transform.position);
                    var exfilContainerBoundingVector = new Vector3(
                        Camera.main.WorldToScreenPoint(point.transform.position).x,
                        Camera.main.WorldToScreenPoint(point.transform.position).y,
                        Camera.main.WorldToScreenPoint(point.transform.position).z);

                    if (exfilContainerBoundingVector.z > 0.01)
                    {
                        GUI.color = Color.green;
                        int distance = (int)distanceToObject;
                        string extractionName = point.name;
                        string extractionSimpleName = extractionNametoSimpleName(extractionName);
                        string boxText = $"{extractionSimpleName} - {distance}m";

                        GUI.Label(new Rect(exfilContainerBoundingVector.x - 50f, (float)Screen.height - exfilContainerBoundingVector.y, 100f, 50f), boxText);
                    }
                }
            }
        }

        private void Init()
        {
            _playerInfo = null;
            _localPlayer = null;
        }

        public void Unload()
        {
            Init();
            Destroy(GameObjectHolder);
            Destroy(GameObjectHolder.GetComponent<Main>());
            Destroy(this);
            DestroyObject(this);
        }
    }
}
