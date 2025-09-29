<script lang="ts" setup>
defineProps(['message'])
</script>
<template>
  <div class="message-item">
    <strong
      >{{ message.source === 'user' ? 'You' : 'Bot' }} (<small style="color: gray">{{
        new Date(message.timestamp).toLocaleString()
      }}</small
      >):</strong
    >
    <p v-html="message.text.replace(/(\r\n|\n|\r)/g, '<br />')"></p>
  </div>
</template>
