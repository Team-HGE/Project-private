using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artngame.INfiniDy {
    public class growGrassGradualAtGameStart : MonoBehaviour
    {
        public bool fullDisableGrassGrowers = false;
        public bool growGradual = false;
        public InfiniGRASSManager grassManager;
        public int growAtOnce = 20;
        int indexStart = 0;
        public int disableAfterXframes = 5;
        int frameCounter = 0;

        public bool applyLayer = false;
        public List<int> layersPerBrush = new List<int>();
        private void LateUpdate()
        {
            if (applyLayer)
            {
                for (int i = 0; i < grassManager.StaticCombiners.Count; i++)
                {
                    ControlCombineChildrenINfiniDyGrass batcher = grassManager.StaticCombiners[i].GetComponent<ControlCombineChildrenINfiniDyGrass>();                    
                    if (batcher.Type - 1 < layersPerBrush.Count && layersPerBrush[batcher.Type - 1] >=0 )
                    {
                        batcher.changeLayer = true;
                        batcher.layerID = layersPerBrush[batcher.Type - 1];
                    }
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (billboardMat != null)
            {
                billboardMat.SetFloat("_BillboardWidth", 0);
                billboardMat.SetFloat("_BillboardHeight", 0);
            }

            if (growGradual)
            {
                grassManager.GradualGrowth = true;
                //grassManager.
            }
        }

        bool finished = false;
        bool finishedDisable = false;

        public Material billboardMat;
        public float billboardWidth = 10;
        public float billboardHeightOffset = 1;

        public bool regulateBathcers = false; //enable disable LOD controllers if very far or by frames
        public bool regulatePerFrame = false;
        public int regulatePerSeconds = 1;
        public int regulateAfterSeconds = 5;
        int currentBatcher = 0;
        Vector3 prevPos = Vector3.zero;
        public float updateThreshold = 0.2f;
        void LateUpdateA()
        {
            if (regulateBathcers)
            {
                //if (regulatePerSeconds != 0)
                //{
                //    for (int i = 0; i < grassManager.StaticCombiners.Count; i++)
                //    {
                //        grassManager.StaticCombiners[i].active = false;
                //    }
                //}
                if (regulatePerFrame && Time.fixedTime > regulateAfterSeconds)
                {                    
                    //for (int i = 0; i < grassManager.StaticCombiners.Count; i++)
                    //{
                    //    if (currentBatcher != i)
                    //    //if((prevPos - Camera.main.transform.position).magnitude > updateThreshold)
                    //    {
                    //        grassManager.StaticCombiners[i].GetComponent<ControlCombineChildrenINfiniDyGrass>().enabled = true;
                    //    }
                    //    else
                    //    {
                    //        //if(Vector2Int.Distance(Camera.main.transform.position, ) )
                    //        grassManager.StaticCombiners[i].GetComponent<ControlCombineChildrenINfiniDyGrass>().enabled = false;
                    //    }
                    //}

                    if ((prevPos - Camera.main.transform.position).magnitude > updateThreshold)
                    {
                        for (int i = 0; i < grassManager.StaticCombiners.Count; i++)
                        {                            
                            grassManager.StaticCombiners[i].GetComponent<ControlCombineChildrenINfiniDyGrass>().enabled = true;                            
                        }
                        prevPos = Camera.main.transform.position;
                    }
                    else
                    {
                        for (int i = 0; i < grassManager.StaticCombiners.Count; i++)
                        {
                            if (!startOptimizeDelay)
                            {
                                grassManager.StaticCombiners[i].GetComponent<ControlCombineChildrenINfiniDyGrass>().enabled = false;
                            }
                            else
                            {
                                grassManager.StaticCombiners[i].GetComponent<ControlCombineChildrenINfiniDyGrass>().enabled = true;
                            }
                        }
                    }

                       
                    currentBatcher++;
                    if(currentBatcher == grassManager.StaticCombiners.Count)
                    {
                        currentBatcher = 0;
                    }
                }
            }

          

            if (fullDisableGrassGrowers && !finishedDisable)
            {
                if (frameCounter > disableAfterXframes)
                {
                    for (int i = 0; i < grassManager.Grasses.Count; i++)
                    {
                        grassManager.Grasses[i].gameObject.SetActive(false);
                    }
                    finishedDisable = true;
                    // this.enabled = false;
                    if (billboardMat != null)
                    {
                        billboardMat.SetFloat("_BillboardWidth", billboardWidth);
                        billboardMat.SetFloat("_BillboardHeight", billboardHeightOffset);
                    }
                }
                frameCounter++;
            }
        }


        //v0.1
        int prevGrassCount = 0;
        public bool startOptimizeDelay = false;
        float delay_timer = 0;
        public float delay_time = 3;

        // Update is called once per frame
        void Update()
        {
            //Stop optimize batchers when palnting new grass
            //if (!startOptimizeDelay)
            {
                if (grassManager.Grasses.Count != prevGrassCount)
                {
                    startOptimizeDelay = true;
                    delay_timer = Time.fixedTime;
                    prevGrassCount = grassManager.Grasses.Count;
                }
            }
            if (startOptimizeDelay && Time.fixedTime - delay_timer > delay_time)
            {
                startOptimizeDelay = false;
            }
            else
            {
               // delay_timer += Time.deltaTime;
            }


            if (growGradual)
            {
                if (indexStart > grassManager.Grasses.Count)
                {
                    if (!finished)
                    {
                        grassManager.GradualGrowth = false;
                        for (int i = 0; i < grassManager.Grasses.Count; i++)
                        {
                            grassManager.Grasses[i].gameObject.SetActive(false);
                        }
                        finished = true;
                    }

                    return;
                }
                //int growNumber = growAtOnce;
                //if (indexStart  + growAtOnce >= grassManager.Grasses.Count)
                //{
                //    growNumber = grassManager.Grasses.Count - 1;
                //    //return;
                //}

                for (int i = indexStart; i < Mathf.Min(indexStart + growAtOnce, grassManager.Grasses.Count - 1); i++)
                {
                    grassManager.Grasses[i].gameObject.SetActive(true);
                }
                indexStart += growAtOnce;
                //Debug.Log(indexStart);
            }

            LateUpdateA();
        }
    }
}