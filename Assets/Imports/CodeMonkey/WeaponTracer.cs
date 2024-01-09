/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this Code Monkey project
    I hope you find it useful in your own projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class WeaponTracer : MonoBehaviour {

    [SerializeField] private static Material weaponTracerMaterial;
    [SerializeField] private Sprite shootFlashSprite;


    public static void Shoot(Vector3 gunEndPointPosition, Vector3 shootPosition) {
        CreateWeaponTracer(gunEndPointPosition, shootPosition);
        //CreateShootFlash(gunEndPointPosition);
        ShakeCamera(.5f, .05f);
    }

    //private void CreateShootFlash(Vector3 spawnPosition) {
    //    World_Sprite worldSprite = World_Sprite.Create(spawnPosition, shootFlashSprite, 10000);
    //    FunctionTimer.Create(worldSprite.DestroySelf, .05f);
    //}

    public static void CreateWeaponTracer(Vector3 fromPosition, Vector3 targetPosition) {
        Vector3 dir = (targetPosition - fromPosition).normalized;
        float eulerZ = UtilsClass.GetAngleFromVectorFloat(dir) - 90;
        float distance = Vector3.Distance(fromPosition, targetPosition);
        Vector3 tracerSpawnPosition = fromPosition + dir * distance * .5f;
        Material tmpWeaponTracerMaterial = new Material(weaponTracerMaterial);
        tmpWeaponTracerMaterial.SetTextureScale("_MainTex", new Vector2(1f, distance / 256f));
        World_Mesh worldMesh = World_Mesh.Create(tracerSpawnPosition, eulerZ, 6f, distance, tmpWeaponTracerMaterial, null, 10000);

        int frame = 0;
        float framerate = .016f;
        float timer = framerate;
        worldMesh.SetUVCoords(new World_Mesh.UVCoords(0, 0, 16, 256));
        FunctionUpdater.Create(() => {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                frame++;
                timer += framerate;
                if (frame >= 4) {
                    worldMesh.DestroySelf();
                    return true;
                } else {
                    worldMesh.SetUVCoords(new World_Mesh.UVCoords(16 * frame, 0, 16, 256));
                }
            }
            return false;
        });
    }



    public static void ShakeCamera(float intensity, float timer) {
        Vector3 lastCameraMovement = Vector3.zero;
        FunctionUpdater.Create(delegate () {
            timer -= Time.unscaledDeltaTime;
            Vector3 randomMovement = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * intensity;
            Camera.main.transform.position = Camera.main.transform.position - lastCameraMovement + randomMovement;
            lastCameraMovement = randomMovement;
            return timer <= 0f;
        }, "CAMERA_SHAKE");
    }

}
