using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    public Camera cam;

    enum InteractionType { DESTROY, BUILD };
    InteractionType interactionType;
    public Block.blockType block;
    public GameObject slot1, slot2, slot3, slot4, slot5;
    bool destroy = false;
    float downTime, upTime, pressTime = 0;
    float countDown = 1.0f;
    public AudioSource pop;
    public AudioSource destroyBlock;

    void Update()
    { 
        chooseBlock();
        if (cam.enabled == true)
        {
            //bool interaction = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1); //se qualquer 1 for premido
            bool interaction = Input.GetMouseButtonDown(1); //se qualquer 1 for premido

            if (interaction)
            {
                interactionType = InteractionType.BUILD;
            }

            if ((Input.GetMouseButtonDown(0)) && destroy == false)
            {
                downTime = Time.time;
                pressTime = downTime + countDown;
                destroy = true;
                if (!destroyBlock.isPlaying)
                {
                    destroyBlock.Play();
                } 
            }
            if (Input.GetMouseButtonUp(0))
            {
                destroyBlock.Stop();
                destroy = false;
            }
            if (Time.time >= pressTime && destroy == true)
            {
                destroy = false;
                interaction = true;
                interactionType = InteractionType.DESTROY;
                destroyBlock.Stop();
            }

            if (interaction)
            {
                //interactionType = Input.GetMouseButtonDown(0) ? InteractionType.DESTROY : InteractionType.BUILD; //esquerdo é 0

                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)) //10 é o alcance
                {
                    //se houve colisão
                    string chunkName = hit.collider.gameObject.name;
                    float chunkX = hit.collider.gameObject.transform.position.x;
                    float chunkY = hit.collider.gameObject.transform.position.y;
                    float chunkZ = hit.collider.gameObject.transform.position.z;

                    Vector3 hitBlock;
                    if (interactionType == InteractionType.DESTROY)
                    {
                        hitBlock = hit.point - hit.normal / 2f;
                    }
                    else
                    {
                        hitBlock = hit.point + hit.normal / 2f;
                    }

                    int blockx = (int)(Mathf.Round(hitBlock.x) - chunkX);
                    int blocky = (int)(Mathf.Round(hitBlock.y) - chunkY);
                    int blockz = (int)(Mathf.Round(hitBlock.z) - chunkZ);

                    Chunk c;

                    if (World.chunkDic.TryGetValue(chunkName, out c))
                    {
                        if(!(blockx > 15 || blocky > 15 || blockz > 15 || blockx < 0 || blocky < 0 || blockz < 0))
                        {
                            if (interactionType == InteractionType.DESTROY)
                            {
                                c.chunkData[blockx, blocky, blockz].SetType(Block.blockType.AIR);
                            }
                            else
                            {
                                pop.Play();
                                c.chunkData[blockx, blocky, blockz].SetType(block);
                            }
                        }
                    }

                    List<string> updates = new List<string>();//os chunks vizinhos que é preciso redesenhar

                    updates.Add(chunkName); //é preciso redesenhar o próprio

                    if (blockx == 0)
                    {
                        updates.Add(World.CreateChunkNames(new Vector3(chunkX - World.chunkSize, chunkY, chunkZ)));
                    }
                    else if (blockx == World.chunkSize - 1)
                    {
                        updates.Add(World.CreateChunkNames(new Vector3(chunkX + World.chunkSize, chunkY, chunkZ)));
                    }
                    if (blocky == 0)
                    {
                        updates.Add(World.CreateChunkNames(new Vector3(chunkX, chunkY - World.chunkSize, chunkZ)));
                    }
                    else if (blocky == World.chunkSize - 1)
                    {
                        updates.Add(World.CreateChunkNames(new Vector3(chunkX, chunkY + World.chunkSize, chunkZ)));
                    }
                    if (blockz == 0)
                    {
                        updates.Add(World.CreateChunkNames(new Vector3(chunkX, chunkY, chunkZ - World.chunkSize)));
                    }
                    else if (blockz == World.chunkSize - 1)
                    {
                        updates.Add(World.CreateChunkNames(new Vector3(chunkX, chunkY, chunkZ + World.chunkSize)));
                    }

                    foreach (string name in updates)
                    {
                        if (World.chunkDic.TryGetValue(name, out c))
                        {
                            DestroyImmediate(c.gochunk.GetComponent<MeshFilter>());
                            DestroyImmediate(c.gochunk.GetComponent<MeshRenderer>());
                            DestroyImmediate(c.gochunk.GetComponent<MeshCollider>());

                            c.DrawChunk();
                        }
                    }
                }
            }
        }
    }

    void chooseBlock()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slot1.SetActive(true);
            slot2.SetActive(false);
            slot3.SetActive(false);
            slot4.SetActive(false);
            slot5.SetActive(false);
            block = Block.blockType.STONE;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            slot1.SetActive(false);
            slot2.SetActive(true);
            slot3.SetActive(false);
            slot4.SetActive(false);
            slot5.SetActive(false);
            block = Block.blockType.GRASS;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            slot1.SetActive(false);
            slot2.SetActive(false);
            slot3.SetActive(true);
            slot4.SetActive(false);
            slot5.SetActive(false);
            block = Block.blockType.SNOW;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            slot1.SetActive(false);
            slot2.SetActive(false);
            slot3.SetActive(false);
            slot4.SetActive(true);
            slot5.SetActive(false);
            block = Block.blockType.SAND;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            slot1.SetActive(false);
            slot2.SetActive(false);
            slot3.SetActive(false);
            slot4.SetActive(false);
            slot5.SetActive(true);
            block = Block.blockType.MOSS;
        }
    }
}
