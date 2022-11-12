using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] GameObject enemy, enemyBullet, enemyBulletParent;
        [SerializeField] private int maxEnemyRows = 8, maxEnemyColumns = 8;
        [SerializeField] float movingTime = 1f, respawnMovingTime = 5f, horizontalSpace = 1f, verticalSpace = 1f, respawnHeight = 5f, enablingDelay = 0.1f;

        private GameObject[][] enemies;
        private int[][] rowColorOcurrences;

        private void Start()
        {
            initialiceEnemies();
        }

        public void resetEnemies()
        {
            for (int i = 0; i < maxEnemyRows; i++)
            {
                rowColorOcurrences[i] = new int[4];
                for (int j = 0; j < 4; j++)
                {
                    rowColorOcurrences[i][j] = maxEnemyColumns / 4;
                }
            }
            foreach (Transform enemy in transform)
            {
                enemy.SendMessage("reset");
            }
        }

        public void verifyRowIntegrity(Enemy.EnemyColor color, int row)
        {
            rowColorOcurrences[row][(int)color]--;
            if (rowColorOcurrences[row][(int)color] == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    rowColorOcurrences[row][i] = maxEnemyColumns / 4;
                }
                killRow(row);
            }
        }

        public Color getColor(Enemy.EnemyColor color)
        {
            return enemy.GetComponent<Enemy>().getColor(color);
        }

        public void disableEnemies()
        {
            foreach (var row in enemies)
            {
                foreach (var enemy in row)
                {
                    enemy.GetComponent<Enemy>().setActive(false, false);
                }
            }
        }

        public void disableShootInEnemies()
        {
            foreach (var row in enemies)
            {
                foreach (var enemy in row)
                {
                    enemy.GetComponent<Enemy>().setIsActive(false);
                }
            }
        }

        private int[] randArrange()
        {
            int[] colors = { 0, 0, 1, 1, 2, 2, 3, 3 };
            int aux, i1, i2, size = colors.Length;
            for (int i = 0; i < size; i++)
            {
                i1 = Random.Range(0, size);
                i2 = Random.Range(0, size);
                if (i2 == i1)
                    i2 = (i2 + 1) % size;
                aux = colors[i1];
                colors[i1] = colors[i2];
                colors[i2] = aux;
            }
            return colors;
        }

        private void initialiceEnemies()
        {
            int[] colors;
            Enemy script;
            enemies = new GameObject[maxEnemyRows][];
            rowColorOcurrences = new int[maxEnemyRows][];

            for (int i = 0; i < maxEnemyRows; i++)
            {
                enemies[i] = new GameObject[maxEnemyColumns];
                colors = randArrange();
                for (int j = 0; j < maxEnemyColumns; j++)
                {
                    enemies[i][j] = Instantiate(
                        enemy,
                        new Vector2(transform.position.x + j * horizontalSpace, transform.position.y - i * verticalSpace),
                        Quaternion.identity, transform
                    );
                    script = enemies[i][j].GetComponent<Enemy>();
                    script.setBulletTemplateAndParent(enemyBullet, enemyBulletParent);
                    script.setColor((Enemy.EnemyColor)colors[j]);
                    script.setRow(i);
                    script.setIsActive(false);
                }
                rowColorOcurrences[i] = new int[4];
                for (int j = 0; j < 4; j++)
                {
                    rowColorOcurrences[i][j] = maxEnemyColumns / 4;
                }
            }
        }

        private void prepareEnemy(int row, Enemy script, float movingTime, Vector2 target, Vector2 startPos = default)
        {
            if (startPos != default)
            {
                script.transform.position = startPos;
            }
            script.moveToTarget(target, movingTime);
            script.setRow(row);
        }

        private void randomizeColors(GameObject[] enemies)
        {
            int[] colors = randArrange();
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<Enemy>().setColor((Enemy.EnemyColor)colors[i]);
            }
        }

        private void setRowActive(bool active, GameObject[] enemies)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<Enemy>().setActive(active, active);
            }
        }

        private void switchRows(int row)
        {
            GameObject[] auxEnemyRow = enemies[row];
            int[] auxOcurrencyRow = rowColorOcurrences[row];

            for (int i = row; i >= 1; i--)
            {
                enemies[i] = enemies[i - 1];
                rowColorOcurrences[i] = rowColorOcurrences[i - 1];
                foreach (var enemy in enemies[i])
                {
                    prepareEnemy(i, enemy.GetComponent<Enemy>(), movingTime,
                        (Vector2)enemy.transform.position + Vector2.down * verticalSpace
                    );
                }
            }
            enemies[0] = auxEnemyRow;
            rowColorOcurrences[0] = auxOcurrencyRow;
        }

        private void setUpFirstRow()
        {
            setRowActive(true, enemies[0]);
            foreach (var enemy in enemies[0])
            {
                prepareEnemy(0, enemy.GetComponent<Enemy>(), respawnMovingTime,
                    new Vector2(enemy.transform.position.x, transform.position.y),
                    new Vector2(enemy.transform.position.x, respawnHeight)
                );
            }
        }

        private void killRow(int row)
        {
            randomizeColors(enemies[row]);
            setRowActive(false, enemies[row]);
            switchRows(row);
            setUpFirstRow();
        }

        public IEnumerator enableEnemies()
        {
            foreach (var row in enemies)
            {
                foreach (var enemy in row)
                {
                    enemy.GetComponent<Enemy>().setActive(true, false);
                    yield return new WaitForSeconds(enablingDelay);
                }
            }
            foreach (var row in enemies)
            {
                foreach (var enemy in row)
                {
                    enemy.GetComponent<Enemy>().setIsActive(true);
                }
            }
        }
    }
}