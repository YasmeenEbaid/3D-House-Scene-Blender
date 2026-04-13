// full name: yasmeen mohamed ebaid
// ID: 2023170698
// Department: SC (scientific computing)
// Section: 5

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Tao.OpenGl;
using GlmNet;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        // buffer lel house vertices
        uint house_buffer_id;
        // buffer lel extra primitives zay axes + ground
        uint extra_primitives_buffer_id;
        // matrices lel transformations
        mat4 HouseModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;

        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;
        int useTextureID;
        // rotation speed lel animation (milestone 2)
        const float rotationSpeed = 1f;
        float rotationAngle = 0;
        // translation bel keyboard (milestone 2)
        public float translationX = 0, translationY = 0, translationZ = 0;
        Stopwatch timer = Stopwatch.StartNew();
        // center beta3 el house (ashan n3mel rotate 7awaleeh)
        vec3 houseCenter;

        float[] houseVertices;
        float[] extraVertices;
        // texture lel ground (milestone 3)
        Texture groundTexture;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader",
                            projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            Gl.glClearColor(0.5f, 0.7f, 1.0f, 1f);
            // enable depth 3shan 3D
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glPointSize(10.0f);

            // ====================== HOUSE VERTICES ======================
            // hena bn3ml 3D house  (milestone 1)
            // kol part leha color mokhtalef
            houseVertices = new float[]
            {
                // 1.hena el WALLS (GL_TRIANGLES) - (0 to 23)
                -2f,0f,-1f, 0.96f,0.87f,0.70f, 0.0f,0.0f,  2f,0f,-1f, 0.96f,0.87f,0.70f, 0.0f,0.0f,  2f,3f,-1f, 0.96f,0.87f,0.70f, 0.0f,0.0f,
                -2f,0f,-1f, 0.96f,0.87f,0.70f, 0.0f,0.0f,  2f,3f,-1f, 0.96f,0.87f,0.70f, 0.0f,0.0f, -2f,3f,-1f, 0.96f,0.87f,0.70f, 0.0f,0.0f,
                // Back
                -2f,0f,-3f, 0.82f,0.71f,0.55f, 0.0f,0.0f,  2f,0f,-3f, 0.82f,0.71f,0.55f, 0.0f,0.0f,  2f,3f,-3f, 0.82f,0.71f,0.55f, 0.0f,0.0f,
                -2f,0f,-3f, 0.82f,0.71f,0.55f, 0.0f,0.0f,  2f,3f,-3f, 0.82f,0.71f,0.55f, 0.0f,0.0f, -2f,3f,-3f, 0.82f,0.71f,0.55f, 0.0f,0.0f,
                // Left
                -2f,0f,-3f, 0.65f,0.50f,0.39f, 0.0f,0.0f, -2f,0f,-1f, 0.65f,0.50f,0.39f, 0.0f,0.0f, -2f,3f,-1f, 0.65f,0.50f,0.39f, 0.0f,0.0f,
                -2f,0f,-3f, 0.65f,0.50f,0.39f, 0.0f,0.0f, -2f,3f,-1f, 0.65f,0.50f,0.39f, 0.0f,0.0f, -2f,3f,-3f, 0.65f,0.50f,0.39f, 0.0f,0.0f,
                // Right
                 2f,0f,-3f, 0.87f,0.72f,0.53f, 0.0f,0.0f,  2f,0f,-1f, 0.87f,0.72f,0.53f, 0.0f,0.0f,  2f,3f,-1f, 0.87f,0.72f,0.53f, 0.0f,0.0f,
                 2f,0f,-3f, 0.87f,0.72f,0.53f, 0.0f,0.0f,  2f,3f,-1f, 0.87f,0.72f,0.53f, 0.0f,0.0f,  2f,3f,-3f, 0.87f,0.72f,0.53f, 0.0f,0.0f,

                // 2.hena el ROOF (GL_TRIANGLES) - (24 to 41)
                -2.5f,3f,-1f, 0.27f,0.51f,0.71f, 0.0f,0.0f,  2.5f,3f,-1f, 0.27f,0.51f,0.71f, 0.0f,0.0f,  0f,5f,-1f, 0.27f,0.51f,0.71f, 0.0f,0.0f,
                -2.5f,3f,-3f, 0.10f,0.30f,0.50f, 0.0f,0.0f,  2.5f,3f,-3f, 0.10f,0.30f,0.50f, 0.0f,0.0f,  0f,5f,-3f, 0.10f,0.30f,0.50f, 0.0f,0.0f,
                -2.5f,3f,-1f, 0.68f,0.85f,0.90f, 0.0f,0.0f, -2.5f,3f,-3f, 0.68f,0.85f,0.90f, 0.0f,0.0f,  0f,5f,-3f, 0.68f,0.85f,0.90f, 0.0f,0.0f,
                -2.5f,3f,-1f, 0.68f,0.85f,0.90f, 0.0f,0.0f,  0f,5f,-3f, 0.68f,0.85f,0.90f, 0.0f,0.0f,  0f,5f,-1f, 0.68f,0.85f,0.90f, 0.0f,0.0f,
                 2.5f,3f,-1f, 0.46f,0.62f,0.80f, 0.0f,0.0f,  2.5f,3f,-3f, 0.46f,0.62f,0.80f, 0.0f,0.0f,  0f,5f,-3f, 0.46f,0.62f,0.80f, 0.0f,0.0f,
                 2.5f,3f,-1f, 0.46f,0.62f,0.80f, 0.0f,0.0f,  0f,5f,-3f, 0.46f,0.62f,0.80f, 0.0f,0.0f,  0f,5f,-1f, 0.46f,0.62f,0.80f, 0.0f,0.0f,

                // 3.hena el  DOOR (GL_TRIANGLES) - (42 to 47)
                -0.5f, 0f, -0.99f, 0.50f, 0.15f, 0.15f, 0.0f, 0.0f,
                 0.5f, 0f, -0.99f, 0.50f, 0.15f, 0.15f, 0.0f, 0.0f,
                 0.5f, 1.8f, -0.99f, 0.50f, 0.15f, 0.15f, 0.0f, 0.0f,
                -0.5f, 0f, -0.99f, 0.50f, 0.15f, 0.15f, 0.0f, 0.0f,
                 0.5f, 1.8f, -0.99f, 0.50f, 0.15f, 0.15f, 0.0f, 0.0f,
                -0.5f, 1.8f, -0.99f, 0.50f, 0.15f, 0.15f, 0.0f, 0.0f,

                // 4.hena el WINDOW 1 (GL_TRIANGLES) - (48 to 53)
                -1.6f, 1.0f, -0.99f, 0.75f, 0.95f, 1.0f, 0.0f, 0.0f,
                -0.8f, 1.0f, -0.99f, 0.75f, 0.95f, 1.0f, 0.0f, 0.0f,
                -0.8f, 2.0f, -0.99f, 0.75f, 0.95f, 1.0f, 0.0f, 0.0f,
                -1.6f, 1.0f, -0.99f, 0.75f, 0.95f, 1.0f, 0.0f, 0.0f,
                -0.8f, 2.0f, -0.99f, 0.75f, 0.95f, 1.0f, 0.0f, 0.0f,
                -1.6f, 2.0f, -0.99f, 0.75f, 0.95f, 1.0f, 0.0f, 0.0f,

                // 5.hena el WINDOW 2 (GL_TRIANGLES) - (54 to 59)
                 0.8f, 1.0f, -0.99f, 0.60f, 0.85f, 1.0f, 0.0f, 0.0f,
                 1.6f, 1.0f, -0.99f, 0.60f, 0.85f, 1.0f, 0.0f, 0.0f,
                 1.6f, 2.0f, -0.99f, 0.60f, 0.85f, 1.0f, 0.0f, 0.0f,
                 0.8f, 1.0f, -0.99f, 0.60f, 0.85f, 1.0f, 0.0f, 0.0f,
                 1.6f, 2.0f, -0.99f, 0.60f, 0.85f, 1.0f, 0.0f, 0.0f,
                 0.8f, 2.0f, -0.99f, 0.60f, 0.85f, 1.0f, 0.0f, 0.0f,

                1.0f, 4.0f, -2.0f, 0.3f, 0.3f, 0.3f, 0.0f, 0.0f,
                1.0f, 4.3f, -2.0f, 0.4f, 0.4f, 0.4f, 0.0f, 0.0f,
                1.0f, 4.6f, -2.0f, 0.5f, 0.5f, 0.5f, 0.0f, 0.0f,
                1.0f, 4.9f, -2.0f, 0.6f, 0.6f, 0.6f, 0.0f, 0.0f,
            };

            houseCenter = new vec3(0, 2, -2);
            house_buffer_id = GPU.GenerateBuffer(houseVertices);
            // hena bn3ml primitives mokhtalefa (milestone 1)
            extraVertices = new float[]
            {
                // 1.hena el XYZ AXES (GL_LINES) - 6 vertices (0 to 5)
                0f,0f,0f, 1f,0f,0f, 0f,0f, 100f,0f,0f, 1f,0f,0f, 0f,0f, 
                0f,0f,0f, 0f,1f,0f, 0f,0f, 0f,100f,0f, 0f,1f,0f, 0f,0f, 
                0f,0f,0f, 0f,0f,1f, 0f,0f, 0f,0f,-100f, 0f,0f,1f, 0f,0f,

                // 2.hena el GROUND (GL_QUADS) - 4 vertices (6 to 9)
                -20f,0f, 20f, 0.0f,0.5f,0.0f,  0.0f,0.0f,
                 20f,0f, 20f, 0.0f,0.5f,0.0f,  1.0f,0.0f,
                 20f,0f,-20f, 0.0f,0.5f,0.0f,  1.0f,1.0f,
                -20f,0f,-20f, 0.0f,0.5f,0.0f,  0.0f,1.0f, 

                // 3.hena el BOUNDING RECTANGLE (GL_LINE_LOOP) - 4 vertices (10 to 13)
                -2.6f, 0f, -0.9f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f,
                 2.6f, 0f, -0.9f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f,
                 2.6f, 0f, -3.1f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f,
                -2.6f, 0f, -3.1f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f
            };
            extra_primitives_buffer_id = GPU.GenerateBuffer(extraVertices);
            //hena b3mel camera view (milestone 1 requirement)
            ViewMatrix = glm.lookAt(new vec3(20, 19, 20), houseCenter, new vec3(0, 1, 0));
            HouseModelMatrix = new mat4(1);
            ProjectionMatrix = glm.perspective(45f, 4f / 3f, 0.1f, 100f);

            sh.UseShader();
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");
            useTextureID = Gl.glGetUniformLocation(sh.ID, "useTexture");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            //hena b3mel load texture lel ground (milestone 3)
            string texturePath = projectPath + "\\Texture\\6.jpg";
            groundTexture = new Texture(texturePath, Gl.GL_TEXTURE0);

            timer.Start();
        }

        public void HandleKeyPress(char key)
        {

            //hena b3mel keyboard control (milestone 2)
            float speed = 5.0f;
            if (key == 'd') translationX += speed;
            if (key == 'a') translationX -= speed;
            if (key == 'w') translationY += speed;
            if (key == 's') translationY -= speed;
            if (key == 'z') translationZ += speed;
            if (key == 'c') translationZ -= speed;
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array());
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, extra_primitives_buffer_id);
            EnableAttributes();
            //hena barsem el house bel primative elmo5tlefa
            //1: GL_LINES (Axes)
            Gl.glUniform1i(useTextureID, 0);
            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            //2: GL_QUADS (Ground)
            Gl.glUniform1i(useTextureID, 1);
            groundTexture.Bind();
            Gl.glDrawArrays(Gl.GL_QUADS, 6, 4);
            groundTexture.Unbind();
            Gl.glUniform1i(useTextureID, 0);

            //3: GL_LINE_LOOP (Fixed Frame)
            Gl.glLineWidth(2.0f);
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 10, 4);
            Gl.glLineWidth(1.0f);

            // Draw Moving House
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, HouseModelMatrix.to_array());
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, house_buffer_id);
            EnableAttributes();

            //4: GL_TRIANGLES (House Body)
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 60);
            //5 GL_POINTS
            Gl.glDrawArrays(Gl.GL_POINTS, 60, 4);

            DisableAttributes();
        }

        private void EnableAttributes()
        {
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));
        }

        private void DisableAttributes()
        {
            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            Gl.glDisableVertexAttribArray(2);
        }

        public void Update()
        {

            //hena b3mel self animation rotation (milestone 2)
            timer.Stop();
            float deltaTime = (float)(timer.ElapsedMilliseconds / 1000.0);
            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transforms = new List<mat4>();
            transforms.Add(glm.translate(new mat4(1), -1 * houseCenter));
            transforms.Add(glm.rotate(rotationAngle, new vec3(0, 1, 0)));
            transforms.Add(glm.translate(new mat4(1), houseCenter));
            transforms.Add(glm.scale(new mat4(1), new vec3(2.4f, 2.4f, 2.4f)));
            //w hena b3mel  keyboard movement
            transforms.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            HouseModelMatrix = MathHelper.MultiplyMatrices(transforms);

            timer.Reset();
            timer.Start();
        }

        public void CleanUp()
        {
            sh.DestroyShader();
            groundTexture.CleanUp();
            Gl.glDeleteBuffers(1, ref house_buffer_id);
            Gl.glDeleteBuffers(1, ref extra_primitives_buffer_id);
        }
    }
}