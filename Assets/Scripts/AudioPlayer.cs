using UnityEngine;

/// <summary>
/// A static utility class for playing one-shot audio clips at a specific position.
/// This creates a temporary GameObject to host the AudioSource and destroys it after the clip finishes.
/// </summary>
public static class AudioPlayer
{
    /// <summary>
    /// Plays an audio clip at a given position in world space.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="position">The world space position to play the sound from.</param>
    /// <param name="volume">The volume of the sound (0.0 to 1.0).</param>
    /// <param name="pitch">The pitch of the sound (1.0 is normal pitch).</param>
    public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1.0f, float pitch = 1.0f)
    {
        // Do nothing if the clip is null, to prevent errors.
        if (clip == null)
        {
            Debug.LogWarning("AudioPlayer was asked to play a null AudioClip.");
            return;
        }

        // 1. Create a new, empty GameObject at the specified position.
        // We give it a name for easier debugging in the scene hierarchy.
        GameObject tempAudioObject = new GameObject("TempAudio");
        tempAudioObject.transform.position = position;

        // 2. Add an AudioSource component to our new GameObject.
        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();

        // 3. Configure the AudioSource settings.
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;

        // Set the sound to be 3D (spatialized) so it sounds like it's coming from 'position'.
        audioSource.spatialBlend = 1.0f;

        // 4. Play the sound.
        audioSource.Play();

        // 5. Schedule the destruction of the temporary GameObject.
        // It will be destroyed after a delay equal to the length of the audio clip.
        Object.Destroy(tempAudioObject, clip.length + 3);
    }
}
