using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD_46
{
    public enum Activities
    {
        IDLE, SLEEPING, GATHERING, FUELING, HUNTING, COOKING, EATING, STARVE, FREEZE
    }

    public class HumanController : MonoBehaviour
    {
        private WorldController m_World;
        private FireController m_Fire;
        private StorageController m_Storage;

        [SerializeField]
        private Slider m_EnergySlider;
        [SerializeField]
        private Slider m_HungerSlider;
        [SerializeField]
        private Slider m_WarmthSlider;
        [SerializeField]
        private Button m_SleepButton;
        [SerializeField]
        private Button m_GatherButton;
        [SerializeField]
        private Button m_HuntButton;
        [SerializeField]
        private Button m_FuelButton;
        [SerializeField]
        private Button m_CookButton;
        [SerializeField]
        private Slider m_ActivitySlider;
        [SerializeField]
        private TextMeshProUGUI m_ActivityText;
        [SerializeField]
        private float m_Energy = 1.0f;
        [SerializeField]
        private float m_Hunger = 1.0f;
        [SerializeField]
        private float m_Warmth = 1.0f;
        [SerializeField]
        private float m_EnergyLossRate = 0.005f;
        [SerializeField]
        private float m_EnergyGainRate = 0.1f;
        [SerializeField]
        private float m_HungerLossRate = 0.01f;
        [SerializeField]
        private float m_HungerGainRate = 0.5f;
        [SerializeField]
        private float m_WarmthLossRate = 0.01f;
        [SerializeField]
        private float m_WarmthGainRate = 0.2f;
        [SerializeField]
        private float m_SleepDuration = 10.0f;
        [SerializeField]
        private float m_GatherDuration = 5.0f;
        [SerializeField]
        private float m_FuelDuration = 1.0f;
        [SerializeField]
        private float m_HuntingDuration = 5.0f;
        [SerializeField]
        private float m_CookingDuration = 2.0f;
        [SerializeField]
        private float m_EatingDuration = 2.0f;
        private Activities m_CurrentActivity = Activities.IDLE;
        private float m_ActivityDuration = 0.0f;

        internal WorldController World { get => m_World; private set => m_World = value; }
        internal FireController Fire { get => m_Fire; private set => m_Fire = value; }
        internal StorageController Storage { get => m_Storage; private set => m_Storage = value; }
        private Slider EnergySlider => m_EnergySlider;
        private Slider HungerSlider => m_HungerSlider;
        private Slider WarmthSlider => m_WarmthSlider;
        public Button SleepButton => m_SleepButton;
        public Button GatherButton => m_GatherButton;
        public Button HuntButton => m_HuntButton;
        public Button FuelButton => m_FuelButton;
        public Button CookButton => m_CookButton;
        public Slider ActivitySlider => m_ActivitySlider;
        public TextMeshProUGUI ActivityText => m_ActivityText;
        public float Energy { get => m_Energy; private set => m_Energy = value; }
        public float Hunger { get => m_Hunger; private set => m_Hunger = value; }
        public float Warmth { get => m_Warmth; private set => m_Warmth = value; }
        private float EnergyLossRate => m_EnergyLossRate;
        public float EnergyGainRate => m_EnergyGainRate;
        private float HungerLossRate => m_HungerLossRate;
        public float HungerGainRate => m_HungerGainRate;
        public float WarmthLossRate => m_WarmthLossRate;
        public float WarmthGainRate => m_WarmthGainRate;
        public float SleepDuration => m_SleepDuration;
        public float GatherDuration => m_GatherDuration;
        public float FuelDuration => m_FuelDuration;
        public float HuntingDuration => m_HuntingDuration;
        public float CookingDuration => m_CookingDuration;
        public float EatingDuration => m_EatingDuration;
        public Activities CurrentActivity { get => m_CurrentActivity; private set => m_CurrentActivity = value; }
        public float ActivityDuration { get => m_ActivityDuration; private set => m_ActivityDuration = value; }

        // Start is called before the first frame update
        void Start()
        {
            World = GetComponent<WorldController>();
            Fire = GetComponent<FireController>();
            Storage = GetComponent<StorageController>();

            CurrentActivity = Activities.IDLE;
            ActivitySlider.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            Energy -= EnergyLossRate * Time.deltaTime;
            Hunger -= HungerLossRate * Time.deltaTime;
            Warmth -= WarmthLossRate * Time.deltaTime / World.Temperature;

            if (CurrentActivity != Activities.HUNTING && CurrentActivity != Activities.GATHERING)
            {
                Warmth += WarmthGainRate * Time.deltaTime * Fire.Heat;
            }

            if (Warmth <= 0.005f)
            {
                BeginActivity(Activities.FREEZE);
            }

            if (Hunger <= 0.005f)
            {
                BeginActivity(Activities.STARVE);
            }

            if (Energy <= 0.005f)
            {
                EndActivity();
                BeginActivity(Activities.SLEEPING);
            }

            UpdateActivity(Time.deltaTime);
            UpdateButtons();

            Energy = Mathf.Clamp(Energy, 0.0f, 1.0f);
            Hunger = Mathf.Clamp(Hunger, 0.0f, 1.0f);
            Warmth = Mathf.Clamp(Warmth, 0.0f, 1.0f);

            EnergySlider.value = Energy;
            HungerSlider.value = Hunger;
            WarmthSlider.value = Warmth;
        }

        private void EnableAllButtons(bool enable)
        {
            SleepButton.enabled = enable;
            GatherButton.enabled = enable;
            HuntButton.enabled = enable;
            FuelButton.enabled = enable;
            CookButton.enabled = enable;
        }

        private void BeginActivity(Activities activity)
        {
            CurrentActivity = activity;
            ActivityDuration = 0.0f;
            EnableAllButtons(false);

            ActivitySlider.gameObject.SetActive(true);
            ActivityText.SetText(GetActivityString(activity));
        }

        private string GetActivityString(Activities activity)
        {
            switch (activity)
            {
                case Activities.SLEEPING:
                    return "Sleeping";
                case Activities.GATHERING:
                    return "Gathering";
                case Activities.FUELING:
                    return "Fueling";
                case Activities.HUNTING:
                    return "Hunting";
                case Activities.COOKING:
                    return "Cooking";
                case Activities.EATING:
                    return "Eating";
                case Activities.STARVE:
                    return "You starve to death!";
                case Activities.FREEZE:
                    return "You freeze to death!";
                default:
                    return "";
            }
        }

        private void UpdateActivity(float deltaTime)
        {
            ActivityDuration += deltaTime;

            switch (CurrentActivity)
            {
                case Activities.SLEEPING:
                    Energy += EnergyGainRate * deltaTime;
                    ActivitySlider.value = ActivityDuration / SleepDuration;
                    if (ActivityDuration >= SleepDuration)
                    {
                        EndActivity();
                    }
                    break;
                case Activities.GATHERING:
                    ActivitySlider.value = ActivityDuration / GatherDuration;
                    if (ActivityDuration >= GatherDuration)
                    {
                        EndGathering();
                        EndActivity();
                    }
                    break;
                case Activities.FUELING:
                    ActivitySlider.value = ActivityDuration / FuelDuration;
                    if (ActivityDuration >= FuelDuration)
                    {
                        EndActivity();
                    }
                    break;
                case Activities.HUNTING:
                    ActivitySlider.value = ActivityDuration / HuntingDuration;
                    if (ActivityDuration >= HuntingDuration)
                    {
                        EndHunting();
                        EndActivity();
                    }
                    break;
                case Activities.COOKING:
                    ActivitySlider.value = ActivityDuration / CookingDuration;
                    if (ActivityDuration >= CookingDuration)
                    {
                        EndActivity();
                        BeginActivity(Activities.EATING);
                    }
                    break;
                case Activities.EATING:
                    Hunger += HungerGainRate * deltaTime;
                    ActivitySlider.value = ActivityDuration / EatingDuration;
                    if (ActivityDuration >= EatingDuration)
                    {
                        EndActivity();
                    }
                    break;
                case Activities.STARVE:
                case Activities.FREEZE:
                    ActivitySlider.value = 1.0f;
                    break;
                default:
                    break;
            }
        }

        private void EndHunting()
        {
            Storage.FoodStored += 1 + Mathf.RoundToInt(Utilities.NextGaussian());
        }

        private void EndGathering()
        {
            Storage.WoodStored += 1 + Mathf.RoundToInt(Utilities.NextGaussian());
        }

        private void EndActivity()
        {
            CurrentActivity = Activities.IDLE;
            ActivityDuration = 0.0f;
            EnableAllButtons(true);

            ActivitySlider.gameObject.SetActive(false);
        }

        public void BeginSleep() { BeginActivity(Activities.SLEEPING); }
        public void BeginGathering() { BeginActivity(Activities.GATHERING); }
        public void BeginFueling() { Storage.WoodStored--; BeginActivity(Activities.FUELING); }
        public void BeginHunting() { BeginActivity(Activities.HUNTING); }
        public void BeginCooking() { Storage.FoodStored--; BeginActivity(Activities.COOKING); }
        public void BeginEating() { BeginActivity(Activities.EATING); }

        private void UpdateButtons()
        {
            if (CurrentActivity != Activities.IDLE) { return; }

            SleepButton.enabled = Energy < 0.5f;
            FuelButton.enabled = (Fire.Heat > 0.005f) && (Storage.WoodStored > 0);
            CookButton.enabled = (Fire.Heat > 0.005f) && (Storage.FoodStored > 0);
        }
    }
}
