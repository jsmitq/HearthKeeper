using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

namespace LD_46
{
    public class FireController : MonoBehaviour
    {
        private WorldController m_World;
        private HumanController m_Human;

        [SerializeField]
        private Slider m_HeatSlider;
        [SerializeField]
        private Slider m_FuelSlider;
        [SerializeField]
        private Light2D m_FireLight1;
        [SerializeField]
        private Light2D m_FireLight2;
        [SerializeField]
        private float m_Heat = 0.5f;
        [SerializeField]
        private float m_Fuel = 1.0f;
        [SerializeField]
        private float m_HeatFluctuationRate = 0.002f;
        [SerializeField]
        private float m_BurnRate = 0.02f;
        [SerializeField]
        private float m_RefuelRate = 0.5f;
        
        internal WorldController World { get => m_World; private set => m_World = value; }
        internal HumanController Human { get => m_Human; private set => m_Human = value; }
        public Slider HeatSlider => m_HeatSlider;
        public Slider FuelSlider => m_FuelSlider;
        public Light2D FireLight1 => m_FireLight1;
        public Light2D FireLight2 => m_FireLight2;
        public float Heat { get => m_Heat; private set => m_Heat = value; }
        public float Fuel { get => m_Fuel; private set => m_Fuel = value; }
        public float HeatFluctuationRate => m_HeatFluctuationRate;
        public float BurnRate => m_BurnRate;
        public float RefuelRate => m_RefuelRate;


        // Start is called before the first frame update
        void Start()
        {
            World = GetComponent<WorldController>();
            Human = GetComponent<HumanController>();
        }

        // Update is called once per frame
        void Update()
        {
            // Update Fuel value
            if (Human.CurrentActivity == Activities.FUELING)
            {
                Fuel += Time.deltaTime * RefuelRate;
            }
            Fuel -= Time.deltaTime * BurnRate;

            // Can't recover heat if fire is out
            if (Heat > 0.005f)
            {
                // Fire gets bigger with more fuel
                Heat += HeatFluctuationRate * (Utilities.NextGaussian() + (Fuel - 0.5f));
            }

            // Clamp values
            Heat = Mathf.Clamp(Heat, 0.0f, 1.0f);
            Fuel = Mathf.Clamp(Fuel, 0.0f, 1.0f);

            // Update slider values
            HeatSlider.value = Heat;
            FuelSlider.value = Fuel;

            if (Heat > 0.005f)
            {
                FireLight1.intensity = 3.0f * Heat +  0.1f * Utilities.NextGaussian();
                FireLight2.intensity = 20.0f * Heat + Utilities.NextGaussian();
            }
            else
            {
                FireLight1.intensity = 0.0f;
                FireLight2.intensity = 0.0f;
            }
        }
    }
}
