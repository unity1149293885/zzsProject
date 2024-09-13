using UnityEngine;
using UnityEngine.SceneManagement;//ע�����������ռ�
using System;
using UnityEngine.UI;

public class LoadingAsync : MonoBehaviour
{
    public Slider slider;//������
    public Text text;//�ı�
    // ���ؽ���
    float loadPro = 0;

    // ���Խ����첽���صķ���ֵ
    AsyncOperation AsyncOp = null;

    //�����ť,��ʼ������һ����,�ı��ͽ�������ʾ���ؽ���
    void StartLoad()
    {
        AsyncOp = SceneManager.LoadSceneAsync("mainScene", LoadSceneMode.Single);//�첽���س�����Ϊ"Demo Valley"�ĳ���,LoadSceneMode.Single��ʾ���������г���
        AsyncOp.allowSceneActivation = false;//allowSceneActivation =true��ʾ����������ɺ��Զ���ת,����,��ֵĬ��Ϊtrue
    }

    void Update()
    {
        if (AsyncOp != null)//����Ѿ���ʼ����
        {
            loadPro = AsyncOp.progress; //��ȡ���ؽ���,�˴��ر�ע��:���س�����progressֵ���Ϊ0.9!!!
        }
        if (loadPro >= 0.9f)//��Ϊprogressֵ���Ϊ0.9,����������Ҫǿ�ƽ������1
        {
            loadPro = 1;
        }
        slider.value = Mathf.Lerp(slider.value, loadPro, 1 * Time.deltaTime);//�������value�Բ�ֵ�ķ�ʽ��������ֵ
        if (slider.value > 0.99f)
        {
            slider.value = 1;
            AsyncOp.allowSceneActivation = true;
        }
        text.text = string.Format("{0:F0}%", slider.value * 100);//�ı����԰ٷֱȵĸ�ʽ��ʾ���ؽ���
    }
}
