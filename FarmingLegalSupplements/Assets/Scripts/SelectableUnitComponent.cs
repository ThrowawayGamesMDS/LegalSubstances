﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectableUnitComponent : MonoBehaviour {
    public Camera camera;
    public GameObject selectionCircle;
    public bool isSelected;
    public WongleController controller;
    public enum workerType {Worker, Ranged, Melee};
    public workerType Type;
    public void Start()
    {
        camera = Camera.main;
    }
    public void Update()
    {
        if (isSelected)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                controller.anim.Play("FlossDance");
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                int layermask = LayerMask.GetMask("Ground", "Enemy", "Building");
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, layermask))
                {
                    switch (hit.transform.tag)
                    {
                        case "Ground":
                            {

                                if (controller.Work != null)
                                {
                                    if (controller.Work.tag == "Building")
                                    {
                                        controller.Work.GetComponent<BuildingController>().worker = null;
                                    }
                                    controller.Work = null;
                                }

                                if(controller.Target != null)
                                {
                                    controller.Target = null;
                                    if (controller.attackinstance != null)
                                    {
                                        Destroy(controller.attackinstance);
                                    }
                                    
                                }
                                controller.agent.isStopped = false;
                                controller.agent.stoppingDistance = 7;
                                controller.agent.SetDestination(hit.point);
                                break;
                            }
                        case "Enemy":
                            {
                                if(Type != workerType.Worker)
                                {
                                    if (controller.Work != null)
                                    {
                                        if (controller.Work.tag == "Building")
                                        {
                                            controller.Work.GetComponent<BuildingController>().worker = null;
                                        }
                                    }
                                    controller.Work = GameObject.FindGameObjectWithTag("Army");
                                    transform.parent = controller.Work.transform;
                                    controller.Target = hit.transform.gameObject;
                                }
                                else
                                {
                                    if (controller.Work != null)
                                    {
                                        if (controller.Work.tag == "Building")
                                        {
                                            controller.Work.GetComponent<BuildingController>().worker = null;
                                        }
                                        controller.Work = null;
                                    }

                                    if (controller.Target != null)
                                    {
                                        controller.Target = null;
                                        if (controller.attackinstance != null)
                                        {
                                            Destroy(controller.attackinstance);
                                        }

                                    }
                                    controller.agent.isStopped = false;
                                    controller.agent.stoppingDistance = 7;
                                    controller.agent.SetDestination(hit.point);
                                }
                                break;
                            }
                        case "Building":
                            {
                                if (Type == workerType.Worker)
                                {
                                    if (hit.transform.GetChild(0).gameObject.GetComponent<BuildingController>() != null)
                                    {
                                        if (hit.transform.GetChild(0).gameObject.GetComponent<BuildingController>().worker == null)
                                        {
                                            controller.isGoingHome = true;
                                            hit.transform.GetChild(0).gameObject.GetComponent<BuildingController>().worker = gameObject;
                                            controller.Work = hit.transform.GetChild(0).gameObject;
                                        }
                                        else
                                        {
                                            controller.agent.SetDestination(hit.point);
                                        }
                                    }
                                }
                                else
                                {
                                    if (controller.Work != null)
                                    {
                                        if (controller.Work.tag == "Building")
                                        {
                                            controller.Work.GetComponent<BuildingController>().worker = null;
                                        }
                                        controller.Work = null;
                                    }

                                    if (controller.Target != null)
                                    {
                                        controller.Target = null;
                                        if (controller.attackinstance != null)
                                        {
                                            Destroy(controller.attackinstance);
                                        }

                                    }
                                    controller.agent.isStopped = false;
                                    controller.agent.stoppingDistance = 7;
                                    controller.agent.SetDestination(hit.point);
                                }
                                break;
                            }
                        case "Wood":
                            {
                                if (Type == workerType.Worker)
                                {
                                    if (controller.Work != null)
                                    {
                                        if (controller.Work.tag == "Building")
                                        {
                                            controller.Work.GetComponent<BuildingController>().worker = null;
                                        }

                                    }
                                    controller.agent.stoppingDistance = 7;
                                    controller.Work = GameObject.FindGameObjectWithTag("WoodCutter");
                                    transform.parent = controller.Work.transform;
                                    controller.Target = hit.transform.gameObject;
                                    controller.isGoingHome = false;
                                }
                                else
                                {
                                    if (controller.Work != null)
                                    {
                                        if (controller.Work.tag == "Building")
                                        {
                                            controller.Work.GetComponent<BuildingController>().worker = null;
                                        }
                                        controller.Work = null;
                                    }

                                    if (controller.Target != null)
                                    {
                                        controller.Target = null;
                                        if (controller.attackinstance != null)
                                        {
                                            Destroy(controller.attackinstance);
                                        }

                                    }
                                    controller.agent.isStopped = false;
                                    controller.agent.stoppingDistance = 7;
                                    controller.agent.SetDestination(hit.point);
                                }
                                break;
                            }
                        case "Crystal":
                            {
                                if (Type == workerType.Worker)
                                {
                                    if (controller.Work != null)
                                    {
                                        if (controller.Work.tag == "Building")
                                        {
                                            controller.Work.GetComponent<BuildingController>().worker = null;
                                        }

                                    }
                                    controller.agent.stoppingDistance = 7;
                                    controller.Work = GameObject.FindGameObjectWithTag("Miner");
                                    transform.parent = controller.Work.transform;
                                    controller.Target = hit.transform.gameObject;
                                    controller.isGoingHome = false;
                                }
                                else
                                {
                                    if (controller.Work != null)
                                    {
                                        if (controller.Work.tag == "Building")
                                        {
                                            controller.Work.GetComponent<BuildingController>().worker = null;
                                        }
                                        controller.Work = null;
                                    }

                                    if (controller.Target != null)
                                    {
                                        controller.Target = null;
                                        if (controller.attackinstance != null)
                                        {
                                            Destroy(controller.attackinstance);
                                        }

                                    }
                                    controller.agent.isStopped = false;
                                    controller.agent.stoppingDistance = 7;
                                    controller.agent.SetDestination(hit.point);
                                }
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }
    }



}
