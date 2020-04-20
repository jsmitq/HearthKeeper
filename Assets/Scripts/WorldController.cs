using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

namespace LD_46
{
    public enum WeatherType
    {
        CLEAR, OVERCAST, RAIN
    }

    public class WorldController : MonoBehaviour
    {
        private HumanController m_Human;
        private FireController m_Fire;

        [SerializeField]
        private Slider m_TemperatureSlider;
        [SerializeField]
        private Slider m_LightSlider;
        [SerializeField]
        private SpriteRenderer m_Sky;
        [SerializeField]
        private Light2D m_GlobalLight;
        [SerializeField]
        private float m_Temperature = 1.0f;
        [SerializeField]
        private WeatherType m_Weather = WeatherType.RAIN;
        [SerializeField]
        private float m_DayTime = 0.5f;
        [SerializeField]
        private float m_Light = 1.0f;
        [SerializeField]
        private float m_TempFluctuationRate = 0.001f;
        [SerializeField]
        private float m_DayLength = 300.0f;

        internal HumanController Human { get => m_Human; private set => m_Human = value; }
        internal FireController Fire { get => m_Fire; private set => m_Fire = value; }
        public Slider TemperatureSlider => m_TemperatureSlider;
        public Slider LightSlider => m_LightSlider;
        public SpriteRenderer Sky => m_Sky;
        public Light2D GlobalLight => m_GlobalLight;
        public float Temperature { get => m_Temperature; private set => m_Temperature = value; }
        public WeatherType Weather { get => m_Weather; private set => m_Weather = value; }
        public float DayTime { get => m_DayTime; private set => m_DayTime = value; }
        public float Light { get => m_Light; private set => m_Light = value; }

        private float TempFluctuationRate => m_TempFluctuationRate;
        public float DayLength => m_DayLength;



        // Start is called before the first frame update
        void Start()
        {
            Human = GetComponent<HumanController>();
            Fire = GetComponent<FireController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Update Light level based on time of day
            DayTime += Time.deltaTime / DayLength;
            DayTime %= 1.0f;
            Light = (1.0f + Mathf.Cos((DayTime * 2.0f * Mathf.PI) + Mathf.PI)) * 0.5f;

            // Update Temperature based on Light level
            Temperature += TempFluctuationRate * (Utilities.NextGaussian() + (Light - 0.5f));

            // Clamp values
            Temperature = Mathf.Clamp(Temperature, 0.1f, 1.0f);
            Light = Mathf.Clamp(Light, 0.0f, 1.0f);

            // Update slider values
            TemperatureSlider.value = Temperature;
            LightSlider.value = Light;

            Sky.color = new Color(Light, Light, Light);
            GlobalLight.intensity = (Light * 2.0f) + 1.0f;
        }
    }
}
