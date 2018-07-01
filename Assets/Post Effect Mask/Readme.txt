Post Effect Mask

Mask any camera post effects. Draws an alpha mask and uses alpha blending to blend the processed image on top of the unprocessed image.

Add a PostEffectMask -component to a camera after the post effect scripts you want to mask.
Add a PostEffectMaskRenderer -component to the objects you want to include in the mask. (Mesh Filter is required)

Parameters:
- Post Effect Mask
  - Opacity:                Control the opacity of the processed picture in the mask.
  - Invert:                 Invert the mask.
  - Blur:                   Level of blur applied on the mask.
  - Capture Mode:           At which point in rendering is the unprocessed image captured.
  
   - Before Opaque Effects:
     - Before effects that are applied at CommandBuffer event BeforeImageEffectsOpaque or later (e.g. Ambient Occlusion in the Post Processing Stack)
     - NOTE: this mode will cause all transparent objects to be masked out
   
   - Before Effects:
     - Before effects that are applied at CommandBuffer event BeforeImageEffects or later (e.g. majority of post effects, Motion Blur, Bloom in the Post Processing Stack)
  
  - Full Screen Texture:    Add the alpha channel of a texture to the mask.
  - Renderers enabled:      Toggle the renderers attached to this mask.
  - Depth test:             Is depth testing done on objects rendered to this mask.
  - Cull mode:              Hide front, back, of no face of the rendered triangles. (Off means that a quad will be visible from both sides)
      
- Post Effect Mask Renderer (parameters for individual objects)
  - Mask:         Which mask is this object rendered to.
  - Opacity:      Opacity used when drawing this object to the mask.
  - Scale:        Scale the vertices of this object when drawing to the mask.
  - Texture:      Texture applied to the mesh when drawing. Only the alpha channel is used!
    
The effect also works in the Scene View. (enable Image Effects through the Toggle Skybox button)