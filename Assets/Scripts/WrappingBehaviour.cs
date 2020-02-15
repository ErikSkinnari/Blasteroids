using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Make this class Singleton??
public static class WrappingBehaviour
{
    public static Vector3 WrappingUpdate(Wrappable thing)
    {
        Vector3 output = thing.transform.position;

        // Check is ship is visible
        var isVisible = CheckRenderers(thing);

        // If ship is visible, no wrapping is happening
        if (isVisible)
        {
            thing.isWrappingX = false;
            thing.isWrappingY = false;
            return output;
        }

        if (thing.isWrappingX && thing.isWrappingY)
        {
            return output;
        }

        var viewportPosition = Camera.main.WorldToViewportPoint(thing.transform.position);

        // Check if the position is outside the camera
        // Check sides.
        if (!thing.isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            // If outside, teleport to the opposite side
            output.x = -output.x;
            thing.isWrappingX = true;
        }
        // Now check the top and bottom
        if (!thing.isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            // If outside, teleport to the opposite side
            output.y = -output.y;
            thing.isWrappingY = true;
        }

        return output;
    }

    private static bool CheckRenderers(Wrappable thing)
    {
        var renderers = thing.GetComponents<Renderer>();

        foreach (var renderer in renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }

        return false;
    }
}
