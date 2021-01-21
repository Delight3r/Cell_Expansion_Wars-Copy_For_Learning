using Manager;
using Spinner;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Models.Cell
{
    public class CellModel : MonoBehaviour
    {
        #region Properties

        public float maxHealth;
        public float currentHealth;

        public int maxOutLines;
        public List<GameObject> currentOutLines;

        public List<GameObject> currentInLines;

        public int level;

        public float sendSpeed;
        public float sendTimer;

        public float healingFrequency;
        public float healingTimer;

        public CellColor color;
        public CellType type;

        public SpriteRenderer sprite;

        public TextMeshProUGUI tmp;

        public bool isSelected;
        public GameObject SelectedSprite;

        public GameObject Seed;

        public List<LineRenderer> LeavingConnections;

        public float multipler;


        #endregion

        #region Start

        private void Start()
        {
            if (this.color == CellColor.Neutral)
                currentHealth = 30;
            
            isSelected = false;

            CalculateStatisticks(type);
            GetColor(color);
            UpdateGUI();
        }

        #endregion

        #region Update

        private void Update()
        {
            sendSpeed = 0.2f;

            sendTimer += Time.deltaTime;
            healingTimer += Time.deltaTime;

            if (sendTimer >= sendSpeed && currentOutLines != null)
                Attack(this.color);

            if (healingTimer >= healingFrequency)
                Heal();

            if (isSelected)
                SelectedSprite.SetActive(true);
            else
                SelectedSprite.SetActive(false);
        }

        #endregion

        #region Attack

        void Attack(CellColor color)
        {

            foreach (GameObject anotherCell in currentOutLines) 
            {
                if (CellManager.amountOfSeeds >= CellManager.maxAmountOfSeeds)
                    return;

                Vector3 target = anotherCell.transform.position;
                target.z = 0f;

                target.x = target.x - transform.position.x;
                target.y = target.y - transform.position.y;

                float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                Instantiate(GetSeed(anotherCell, this.sprite.color), transform.position, rotation);

                CellManager.amountOfSeeds++;
            }

            sendTimer = 0f;
        }

        #endregion

        #region Heal

        void Heal(bool reset = true)
        {
            if (currentHealth < maxHealth && this.color != CellColor.Neutral)
            {
                currentHealth++;
                UpdateGUI();
            }
            else if (currentHealth == maxHealth && this.color != CellColor.Neutral)
                Attack(color);

            if (reset)
                healingTimer = 0f;
        }

        #endregion

        #region TakeHit

        public void TakeHit(CellColor Icolor, float damage)
        {
            if(Icolor != this.color)
                currentHealth--;
            
            if (Icolor == this.color)
                Heal(false);

            if (currentHealth <= 0)
            {
                Capture(Icolor);
            }

            UpdateGUI();
        }

        #endregion

        #region Capture

        void Capture(CellColor color)
        {
            this.color = color;
            GetColor(color);
            currentHealth = maxHealth / 10;

            if(LeavingConnections.Count > 0)
            {
                foreach (LineRenderer lr in LeavingConnections)
                    Destroy(lr);
            }

            currentOutLines.Clear();
        }

        #endregion

        #region UpdateGUI

        void UpdateGUI()
        {
            this.tmp.text = currentHealth.ToString();
        }

        #endregion

        #region Recalculate Statisticks //Not in use yet

        void CalculateStatisticks(CellType type)
        {
            //Not in use yet
            switch (type)
            {
                case CellType.AttackingCell:
                    {
                        
                    }
                    break;
                case CellType.HealingCell:
                    {

                    }
                    break;

                default:
                    {

                    }
                    break;
            }
        }

        #endregion

        #region GetColor

        void GetColor(CellColor color)
        {
            Color32 customBlue = new Color(25/255f, 75/255f, 155/255f);
            Color32 customOrange = new Color(190/255f, 120/255f, 55/255f);
            Color32 customRed = new Color(155/255f, 25f/255f, 25/255f);
            Color32 customGrey = new Color(115/255f, 115/255f, 115/255f);
            Color32 customGreen = new Color(55/255f, 175/255f, 60/255f);

            switch (color)
            {
                case CellColor.Blue:
                    sprite.color = customBlue;
                    break;

                case CellColor.Orange:
                    sprite.color = customOrange;
                    break;

                case CellColor.Player:
                    sprite.color = customGreen;
                    break;

                case CellColor.Red:
                    sprite.color = customRed;
                    break;

                case CellColor.Neutral:
                    sprite.color = customGrey;
                    break;
            }
        }

        Seed GetSeed(GameObject target, Color spriteColor)
        {
            GameObject seed = Seed as GameObject;
            Seed seedd = seed.GetComponent<Seed>();

            seedd.color = this.color;
            seedd.GetComponent<SpriteRenderer>().color = spriteColor;

            seedd.target = target;
            seedd.damage = 1;

            return seedd;
        }

        #endregion

        public enum CellType
        {
            ReguralCell = 0,
            HealingCell = 1,
            AttackingCell = 2
        }

        public enum CellColor
        {
            Player = 0,
            Red = 1,
            Blue = 2,
            Orange = 3,
            Neutral = 4
        }
    }
}
